using PMS.Infrastructure.IoC;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using PMS.WebApi;
using PMS.WebApi.Endpoints;
using SharedKernel;
using SharedKernel.DomainDrivenDesign.Domain.Extensions;
using SharedKernel.ExceptionHandling.Extensions;
using SharedKernel.Mediator.Extensions;
using SharedKernel.Modularity.Extensions;
using SharedKernel.Tenants.Extensions;
using SharedKernel.Tenants.Swaggers.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddSwaggerGen(c =>
{
    c.ConfigureTenant();

    // Add the security definition for JWT Bearer
    c.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description =
            "Enter 'Bearer' [space] and then your valid JWT token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIs...\""
    });

    // Add the security requirement to include the Bearer token globally
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            // Put the security scheme you defined above
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "bearer"
                }
            },
            Array.Empty<string>() // No scopes needed
        }
    });
});




builder.Services.AddKernel(c =>
{
    c.AddTenant();
    c.AddPublicCors();
    c.AddKernelMediator();
    c.AddKernelDomain();
    c.AddModularity<WebApiModules>();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseKernel(c =>
{
    c.UseHttpsRedirection();
    c.UsePublicCors();
    c.UseExceptionHandlingMiddleware();
    c.UseTenant();
});
app.UseAuthentication();
app.UseAuthorization();

await app.UseCore();
app.MapAuthenticationEndpoints();
app.MapInvitationEndpoints();
app.MapTenantEndpoints();
app.MapTenantRolesEndpoints();
app.MapProjectEndpoints();
app.MapSprintEndpoints();
app.MapBoardEndpoints();
app.MapTaskEndpoints();
app.Run();