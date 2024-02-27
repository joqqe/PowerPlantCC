using PowerplantCC.Api.Calculators;
using PowerplantCC.Api.Dtos;

namespace PowerplantCC.Api.Endpoints
{
    internal static class ProductionPlanEndpoints
    {
        public static WebApplication MapProductionPlanEndpoints(this WebApplication app)
        {
            app.MapPost("/productionplan", (ProductionPlan productionPlan) => {
                return PowerDistributionCalculator.Invoke(productionPlan);
            })
            .WithName("GetProductionPlan")
            .WithOpenApi();

            return app;
        } 
    }
}
