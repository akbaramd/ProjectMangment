using PMS.Infrastructure.IoC;
using Microsoft.OpenApi.Models;
using PMS.WebApi;
using PMS.WebApi.Endpoints;

var builder = BonyanApplication.CreateApplicationBuilder<WebApiModules>(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpContextAccessor();
// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddSwaggerGen(c =>
{
   

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





var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();


app.UseAuthentication();
app.UseAuthorization();

await app.UseCore();
app.MapAuthenticationEndpoints();
app.MapTenantManagementEndpoints();

app.Run();