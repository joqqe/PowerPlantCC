using PowerplantCC.Api.Common;
using PowerplantCC.Api.Models;

namespace PowerplantCC.Api.Dtos
{
    public class ProductionPlan
    {
        public decimal Load { get; set; }
        public Fuels Fuels { get; set; } = default!;
        public PowerPlant[] PowerPlants { get; set; } = default!;

        public Result Validate()
        {
            // Load
            if (Load < 0)
                return Result.Error(new ArgumentOutOfRangeException(nameof(Load)));

            // Fuels
            if (Fuels is null)
                return Result.Error(new ArgumentNullException(nameof(Fuels)));

            var fuelsValidateResult = Fuels.Validate();
            if (!fuelsValidateResult.IsSuccess)
                return Result.Error(fuelsValidateResult.Exception!);

            // PowerPlants
            if (PowerPlants is null)
                return Result.Error(new ArgumentNullException(nameof(PowerPlants)));

            var firstInvalidPowerPlantValidateResult = PowerPlants
                .Select(p => p.Validate())
                .FirstOrDefault(r => !r.IsSuccess);
            if (firstInvalidPowerPlantValidateResult is not null)
                return Result.Error(firstInvalidPowerPlantValidateResult.Exception!);

            return Result.Success();
        }
    }
}
