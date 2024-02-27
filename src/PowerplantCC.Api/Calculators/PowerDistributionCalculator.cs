using PowerplantCC.Api.Dtos;
using PowerplantCC.Api.Models;
namespace PowerplantCC.Api.Calculators
{
    public static class PowerDistributionCalculator
    {
        public static LoadedPowerPlant[] Invoke(ProductionPlan productionPlan)
        {
            var loadedPowerPlants = productionPlan.PowerPlants
                .Select(p => new LoadedPowerPlant(p.Name))
                .ToArray();

            // Get Unit efficiency
            var efficiencyOrderedPowerPlants = productionPlan.PowerPlants
                .OrderByDescending(p => p.GetUnitEfficiency(productionPlan.Fuels))
                .ToArray();

            // Load most efficient powerplants first


            // Handle equal load

            return [.. loadedPowerPlants.OrderByDescending(p => p.PowerDelivery)];
        }
    }
}
