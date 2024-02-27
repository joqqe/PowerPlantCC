namespace PowerplantCC.Api.Models
{
    public class PowerPlant
    {
        public string Name { get; set; } = default!;
        public PowerPlantType Type { get; set; }
        public decimal Efficiency { get; set; }
        public decimal PMax { get; set; }
        public decimal PMin { get; set; }
    }
}
