using PowerplantCC.Api.Common;
using PowerplantCC.Api.Dtos;
using PowerplantCC.Api.Models;

namespace PowerplantCC.Api.Calculators
{
    public static class PowerDistributionCalculator
    {
        const int CalculateRunLimit = 4;

        public static Result<LoadedPowerPlant[]> Invoke(ProductionPlan productionPlan)
        {
            // Validation
            if (productionPlan is null)
                return Result<LoadedPowerPlant[]>.Error(new ArgumentNullException(nameof(productionPlan)));

            var validateResult = productionPlan.Validate();
            if (!validateResult.IsSuccess)
                return Result<LoadedPowerPlant[]>.Error(validateResult.Exception!);

            // Get unit efficiency
            var loadedPowerPlantByPowerPlant = productionPlan.PowerPlants
                .OrderByDescending(p => p.GetUnitEfficiency(productionPlan.Fuels))
                .ToDictionary<PowerPlant, LoadedPowerPlant>(p => new LoadedPowerPlant(p.Name));

            for (int i = 0; i < CalculateRunLimit; i++)
            {
                var sumOfAppliedLoad = 0m;

                // Apply most efficient powerplants first
                foreach (KeyValuePair<LoadedPowerPlant, PowerPlant> item in loadedPowerPlantByPowerPlant)
                {
                    sumOfAppliedLoad = GetSumOfAppliedLoad(loadedPowerPlantByPowerPlant);
                    if (sumOfAppliedLoad >= productionPlan.Load)
                        break;

                    var nettoPMin = item.Value.GetNettoLoad(productionPlan.Fuels, p => p.PMin);
                    var nettoPMax = item.Value.GetNettoLoad(productionPlan.Fuels, p => p.PMax);

                    // Apply max powerplant power
                    if (sumOfAppliedLoad + nettoPMax <= productionPlan.Load)
                        item.Key.PowerDelivery += nettoPMax;
                    // Apply partial powerplant power
                    else if (sumOfAppliedLoad + nettoPMin <= productionPlan.Load
                        && sumOfAppliedLoad + nettoPMax >= productionPlan.Load)
                        item.Key.PowerDelivery = productionPlan.Load - sumOfAppliedLoad;
                    // Apply more due to high PMin
                    else if(sumOfAppliedLoad + nettoPMin <= productionPlan.Load)
                        item.Key.PowerDelivery += nettoPMin;
                }

                // Apply rest load


            }

            return Result<LoadedPowerPlant[]>.Success(
                [
                    .. loadedPowerPlantByPowerPlant
                        .Select(p => p.Key)
                        //.OrderByDescending(p => p.PowerDelivery)
                ]);
        }

        private static decimal GetSumOfAppliedLoad(Dictionary<LoadedPowerPlant, PowerPlant> loadedPowerPlantByPowerPlant)
        {
            return loadedPowerPlantByPowerPlant.Sum(p => p.Key.PowerDelivery);
        }
    }
}
