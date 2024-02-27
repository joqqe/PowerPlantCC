using System.Text.Json.Serialization;

namespace PowerplantCC.Api.Models
{
    public class Fuels
    {
        [JsonPropertyName("gas(euro/MWh)")]
        public decimal GasPrice { get; set; }

        [JsonPropertyName("kerosine(euro/MWh)")]
        public decimal KerosinePrice { get; set; }

        [JsonPropertyName("co2(euro/ton)")]
        public decimal Co2Price { get; set; }

        [JsonPropertyName("wind(%)")]
        public decimal WindPercentage { get; set; }
    }
}
