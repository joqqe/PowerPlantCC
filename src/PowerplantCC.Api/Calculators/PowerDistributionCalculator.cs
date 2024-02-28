﻿using PowerplantCC.Api.Common;
using PowerplantCC.Api.Dtos;
using PowerplantCC.Api.Models;

namespace PowerplantCC.Api.Calculators
{
    public static class PowerDistributionCalculator
    {
        public static Result<LoadedPowerPlant[]> Invoke(ProductionPlan productionPlan)
        {
            // Validation
            if (productionPlan is null)
                return Result<LoadedPowerPlant[]>.Error(new ArgumentNullException(nameof(productionPlan)));

            var validateResult = productionPlan.Validate();
            if (!validateResult.IsSuccess)
                return Result<LoadedPowerPlant[]>.Error(validateResult.Exception!);

            // Get unit efficiency
            var powerPlantByLoadedPowerPlant = productionPlan.PowerPlants
                .OrderByDescending(p => p.GetUnitEfficiency(productionPlan.Fuels))
                .ToDictionary<PowerPlant, LoadedPowerPlant>(p => new LoadedPowerPlant(p.Name));

            // Find PowerPlant to start
            var powerPlantsToStart = FindPowerPlantsToStart(productionPlan, powerPlantByLoadedPowerPlant);

            if (powerPlantsToStart?.Count is null or 0)
                return Result<LoadedPowerPlant[]>.Error(
                    new Exception("No combination of the power plants will result in the production of the required load."));

            // Apply min power to all powerplants to start
            ApplyMinPower(productionPlan, powerPlantsToStart);

            // Ramp up most efficient power plants first until load is reached
            RampingUpMostEfficientPowerPlantsFirst(productionPlan, powerPlantsToStart);

            // Merge powerPlantByLoadedPowerPlant with powerPlantsToStart
            MergeUnusedPowerPlantsWithTheStartedOnce(powerPlantByLoadedPowerPlant, powerPlantsToStart);

            return Result<LoadedPowerPlant[]>.Success([.. powerPlantByLoadedPowerPlant.Select(p => p.Key)]);
        }

        private static Dictionary<LoadedPowerPlant, PowerPlant> FindPowerPlantsToStart(ProductionPlan productionPlan, Dictionary<LoadedPowerPlant, PowerPlant> powerPlantByLoadedPowerPlant)
        {
            // Som min power < Load < Som max power
            var powerPlantsToStart = new Dictionary<LoadedPowerPlant, PowerPlant>();

            for (int i = 0; i < powerPlantByLoadedPowerPlant.Count; i++)
            {
                decimal somMinPower = 0m;
                decimal somMaxPower = 0m;

                powerPlantsToStart = [];

                for (int j = i; j < powerPlantByLoadedPowerPlant.Count; j++)
                {
                    var powerPlant = powerPlantByLoadedPowerPlant.ElementAt(j);

                    somMinPower += powerPlant.Value.GetNettoLoad(productionPlan.Fuels, p => p.PMin);
                    somMaxPower += powerPlant.Value.GetNettoLoad(productionPlan.Fuels, p => p.PMax);

                    // Found it!
                    if (somMinPower < productionPlan.Load
                        && productionPlan.Load < somMaxPower)
                    {
                        powerPlantsToStart.Add(powerPlant.Key, powerPlant.Value);
                        break;
                    }

                    // Not yet...
                    if (somMinPower < productionPlan.Load
                        && productionPlan.Load > somMaxPower)
                    {
                        powerPlantsToStart.Add(powerPlant.Key, powerPlant.Value);
                        continue;
                    }

                    // Not possible, som of min power to high :(
                    if (somMinPower > productionPlan.Load
                        && productionPlan.Load < somMaxPower)
                    {
                        powerPlantsToStart.Clear();
                        break;
                    }
                }

                if (powerPlantsToStart is not null)
                    break;
            }

            return powerPlantsToStart!;
        }

        private static void RampingUpMostEfficientPowerPlantsFirst(ProductionPlan productionPlan, Dictionary<LoadedPowerPlant, PowerPlant> powerPlantsToStart)
        {
            foreach (var powerplant in powerPlantsToStart)
            {
                var sumOfAppliedLoad = powerPlantsToStart.Sum(p => p.Key.PowerDelivery);
                var nettoPMax = powerplant.Value.GetNettoLoad(productionPlan.Fuels, p => p.PMax);
                var nettoPMin = powerplant.Value.GetNettoLoad(productionPlan.Fuels, p => p.PMin);

                if (sumOfAppliedLoad == productionPlan.Load)
                    break;
                else if (sumOfAppliedLoad + nettoPMax <= productionPlan.Load)
                    powerplant.Key.PowerDelivery = nettoPMax;
                else if (productionPlan.Load - sumOfAppliedLoad <= nettoPMax - nettoPMin)
                    powerplant.Key.PowerDelivery += productionPlan.Load - sumOfAppliedLoad;
            }
        }

        private static void ApplyMinPower(ProductionPlan productionPlan, Dictionary<LoadedPowerPlant, PowerPlant> powerPlantsToStart)
        {
            foreach (var powerplant in powerPlantsToStart)
            {
                powerplant.Key.PowerDelivery = powerplant.Value.GetNettoLoad(productionPlan.Fuels, p => p.PMin);
            }
        }

        private static void MergeUnusedPowerPlantsWithTheStartedOnce(Dictionary<LoadedPowerPlant, PowerPlant> powerPlantByLoadedPowerPlant, Dictionary<LoadedPowerPlant, PowerPlant> powerPlantsToStart)
        {
            foreach (var powerPlant in powerPlantByLoadedPowerPlant)
            {
                powerPlantsToStart.TryAdd(powerPlant.Key, powerPlant.Value);
            }
        }
    }
}
