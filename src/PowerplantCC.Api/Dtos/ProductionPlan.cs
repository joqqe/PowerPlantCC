using PowerplantCC.Api.Models;

namespace PowerplantCC.Api.Dtos
{
    public class ProductionPlan
    {
        public decimal Load { get; set; }
        public Fuels Fuels { get; set; } = default!;
        public PowerPlant[] PowerPlants { get; set; } = default!;
    }
}
