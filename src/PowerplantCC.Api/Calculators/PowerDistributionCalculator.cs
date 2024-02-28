using PowerplantCC.Api.Common;
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

            // Init return value
            var loadedPowerPlants = productionPlan.PowerPlants
                .Select(p => new LoadedPowerPlant(p.Name))
                .ToArray();

            // Get unit efficiency
            var efficiencyOrderedPowerPlants = productionPlan.PowerPlants
                .OrderByDescending(p => p.GetUnitEfficiency(productionPlan.Fuels))
                .ToArray();

            // Load most efficient powerplants first


            // Handle equal load


            return Result<LoadedPowerPlant[]>.Success(
                [.. loadedPowerPlants.OrderByDescending(p => p.PowerDelivery)]);
        }
    }
}
