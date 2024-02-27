namespace PowerplantCC.Api.Models
{
    internal static class PowerPlantExtensions
    {
        public static FuelType GetFuelType(this PowerPlant powerPlant)
        {
            return powerPlant.Type switch
            {
                PowerPlantType.Gasfired => FuelType.Gas,
                PowerPlantType.Turbojet => FuelType.Kerosine,
                PowerPlantType.Windturbine => FuelType.Wind,
                _ => throw new NotImplementedException($"Power plant type {powerPlant.Type} not implemented.")
            };
        }

        public static EmissionType GetEmissionType(this PowerPlant powerPlant)
        {
            return powerPlant.Type switch
            {
                PowerPlantType.Gasfired => EmissionType.Co2,
                PowerPlantType.Turbojet => EmissionType.Co2,
                PowerPlantType.Windturbine => EmissionType.None,
                _ => throw new NotImplementedException($"Power plant type {powerPlant.Type} not implemented.")
            };
        }

        public static decimal GetFuelCost(this PowerPlant powerPlant, Fuels fuels)
        {
            return powerPlant.GetFuelType() switch
            {
                FuelType.Gas => fuels.GasPrice,
                FuelType.Kerosine => fuels.KerosinePrice,
                FuelType.Wind => fuels.WindPercentage / 100m,
                _ => throw new NotImplementedException()
            };
        }

        public static decimal GetUnitEfficiency(this PowerPlant powerPlant, Fuels fuels)
        {
            return powerPlant.Efficiency / powerPlant.GetFuelCost(fuels);
        }
    }
}
