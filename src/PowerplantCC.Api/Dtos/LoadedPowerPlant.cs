using System.Text.Json.Serialization;

namespace PowerplantCC.Api.Dtos
{
    public class LoadedPowerPlant
    {
        public string Name { get; set; } = default!;

        [JsonPropertyName("p")]
        public decimal PowerDelivery { get; set; } // Should be multiple of 0.1 Mw!

        public LoadedPowerPlant()
        { }

        public LoadedPowerPlant(string name)
        {
            Name = name;
        }
    }
}
