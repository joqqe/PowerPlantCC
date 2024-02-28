using PowerplantCC.Api.Common;
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

        public Result Validate()
        {
            if (GasPrice < 0)
                return Result.Error(new ArgumentOutOfRangeException(nameof(GasPrice), "Gas price should not be lower then 0."));

            if (KerosinePrice < 0)
                return Result.Error(new ArgumentOutOfRangeException(nameof(KerosinePrice), "Kerosine price should not be lower then 0."));

            if (Co2Price < 0)
                return Result.Error(new ArgumentOutOfRangeException(nameof(Co2Price), "Co2 price should not be lower then 0."));

            if (WindPercentage < 0 || WindPercentage > 100)
                return Result.Error(new ArgumentOutOfRangeException(nameof(WindPercentage), "Wind percentage should be between 0 and 100."));

            return Result.Success();
        }
    }
}