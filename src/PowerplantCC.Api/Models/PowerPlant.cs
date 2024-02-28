using PowerplantCC.Api.Common;

namespace PowerplantCC.Api.Models
{
    public class PowerPlant
    {
        public string Name { get; set; } = default!;
        public PowerPlantType Type { get; set; }
        public decimal Efficiency { get; set; }
        public decimal PMax { get; set; }
        public decimal PMin { get; set; }

        public Result Validate()
        {
            if (string.IsNullOrWhiteSpace(Name))
                return Result.Error(new ArgumentNullException(nameof(Name)));

            if (Efficiency < 0 || Efficiency > 1)
                return Result.Error(new ArgumentOutOfRangeException(nameof(Efficiency), "Efficiency should be between 0 and 1."));

            if (PMin < 0)
                return Result.Error(new ArgumentOutOfRangeException(nameof(PMin), "Minimum power of a plant should not be lower then 0."));

            if (PMax < 0)
                return Result.Error(new ArgumentOutOfRangeException(nameof(PMax), "Minimum power of a plant should not be lower then 0."));

            if (PMin > PMax)
                return Result.Error(new ArgumentException("The maximum power should be higher then the minimum power."));

            return Result.Success();
        }
    }
}
