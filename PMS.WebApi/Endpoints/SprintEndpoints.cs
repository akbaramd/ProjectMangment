using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PMS.Application.DTOs;
using PMS.Application.Interfaces;
using SharedKernel.Extensions;
using SharedKernel.Tenants.Abstractions;
using System.Security.Claims;

namespace PMS.WebApi.Endpoints
{
    public static class SprintEndpoints
    {
        public static WebApplication MapSprintEndpoints(this WebApplication app)
        {
            var sprintGroup = app.MapGroup("/api/sprints")
                .WithTags("Sprints");

            // Get sprints by project ID (tenant required)
            sprintGroup.MapGet("/by-projects/{projectId:guid}", [Authorize] async (
                Guid projectId,
                [FromServices] ISprintService sprintService,
                [FromServices] ITenantAccessor tenantAccessor,
                ClaimsPrincipal user) =>
            {
                if (tenantAccessor.Tenant == null)
                {
                    return Results.BadRequest("Tenant is required.");
                }

                var userId = Guid.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);
                var tenantId = tenantAccessor.Tenant;

                var sprints = await sprintService.GetSprintsByProjectIdAsync(projectId, tenantId, userId);
                return Results.Ok(sprints);
            })
            .Produces<List<SprintDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .RequiredTenant();

            return app;
        }
    }
}
