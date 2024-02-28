using PowerplantCC.Api.Dtos;
using PowerplantCC.Api.Models;
using PowerplantCC.Api.Calculators;

namespace PowerplantCC.ApiTests.Calculators
{
    public class PowerDistributionCalculatorTests
    {
        public static IEnumerable<object[]> GetProductionPlan()
        {
            yield return new object[]
            {
                new ProductionPlan
                {
                    Load = 910m,
                    Fuels = new Fuels
                    {
                        GasPrice = 13.4m,
                        KerosinePrice = 50.8m,
                        Co2Price = 20m,
                        WindPercentage = 60m
                    },
                    PowerPlants =
                    [
                        new PowerPlant
                        {
                            Name = "gasfiredbig1",
                            Type = PowerPlantType.Gasfired,
                            Efficiency = 0.53m,
                            PMax = 460m,
                            PMin = 100m
                        },
                        new PowerPlant
                        {
                            Name = "gasfiredbig2",
                            Type = PowerPlantType.Gasfired,
                            Efficiency = 0.53m,
                            PMax = 460m,
                            PMin = 100m
                        },
                        new PowerPlant
                        {
                            Name = "gasfiredsomewhatsmaller",
                            Type = PowerPlantType.Gasfired,
                            Efficiency = 0.37m,
                            PMax = 210m,
                            PMin = 40m
                        },
                        new PowerPlant
                        {
                            Name = "tj1",
                            Type = PowerPlantType.Turbojet,
                            Efficiency = 0.3m,
                            PMax = 16m,
                            PMin = 0m
                        },
                        new PowerPlant
                        {
                            Name = "windpark1",
                            Type = PowerPlantType.Windturbine,
                            Efficiency = 1m,
                            PMax = 150m,
                            PMin = 0m
                        },
                        new PowerPlant
                        {
                            Name = "windpark2",
                            Type = PowerPlantType.Windturbine,
                            Efficiency = 1m,
                            PMax = 36m,
                            PMin = 0m
                        }
                    ]
                },
                new LoadedPowerPlant[]
                {
                    new() {
                        Name = "windpark1",
                        PowerDelivery = 90.0m,
                    },
                    new()
                    {
                        Name = "windpark2",
                        PowerDelivery = 21.6m,
                    },
                    new()
                    {
                        Name = "gasfiredbig1",
                        PowerDelivery = 460m,
                    },
                    new()
                    {
                        Name = "gasfiredbig2",
                        PowerDelivery = 338.4m,
                    },
                    new()
                    {
                        Name = "gasfiredsomewhatsmaller",
                        PowerDelivery = 0m,
                    },
                    new()
                    {
                        Name = "tj1",
                        PowerDelivery = 0m,
                    }
                }
            };
        }

        [Theory]
        [MemberData(nameof(GetProductionPlan))]
        public void Invoke_ShouldReturn_ArrayOfLoadedPowerplants(
            ProductionPlan productionPlan, LoadedPowerPlant[] expectedLoadedPowerPlants)
        {
            // act
            var result = PowerDistributionCalculator.Invoke(productionPlan);

            PowerDistributionCalculator.Invoke(productionPlan);

            // assert
            if (!result.IsSuccess)
                Assert.Fail(result.Exception!.Message);

            LoadedPowerPlant[] actualLoadedPowerPlants = result.Value!;

            Assert.Equal(expectedLoadedPowerPlants.Length, actualLoadedPowerPlants.Length);
            for (int i = 0; i < expectedLoadedPowerPlants.Length; i++)
            {
                var expectedLoadedPowerPlant = expectedLoadedPowerPlants[i];
                var actualLoadedPowerPlant = actualLoadedPowerPlants[i];
                Assert.Equal(expectedLoadedPowerPlant.Name, actualLoadedPowerPlant.Name);
                Assert.Equal(expectedLoadedPowerPlant.PowerDelivery, actualLoadedPowerPlant.PowerDelivery);
            }
        }
    }
}
