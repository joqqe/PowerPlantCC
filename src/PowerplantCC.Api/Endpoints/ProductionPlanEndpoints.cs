using PowerplantCC.Api.Calculators;
using PowerplantCC.Api.Dtos;

namespace PowerplantCC.Api.Endpoints
{
    internal static class ProductionPlanEndpoints
    {
        public static WebApplication MapProductionPlanEndpoints(this WebApplication app)
        {
            app.MapPost("/productionplan", (ProductionPlan productionPlan) => {
                var result = PowerDistributionCalculator.Invoke(productionPlan);

                if (!result.IsSuccess)
                    return Results.BadRequest(result.Exception!.Message);

                if (result?.Value?.Length is null or 0)
                    return Results.NoContent();

                return Results.Ok(result.Value);
            })
            .WithName("GetProductionPlan")
            .WithOpenApi();

            return app;
        } 
    }
}
