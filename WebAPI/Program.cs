using Application;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using WebAPI.Errors;
using WebAPI.Security;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var problem = new ValidationProblemDetails(context.ModelState)
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "Request validation failed",
            Type = "https://httpstatuses.com/400"
        };
        problem.Extensions["code"] = "ValidationError";
        return new BadRequestObjectResult(problem)
        {
            ContentTypes = { "application/problem+json" }
        };
    };
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    if (builder.Environment.IsDevelopment())
    {
        // Dev identity headers (see MatrixIdentityExtensions): pick a role without logging in.
        var devRequirement = new OpenApiSecurityRequirement();
        foreach (var header in new[] { "X-Dev-UserId", "X-Dev-Role", "X-Dev-BranchId" })
        {
            options.AddSecurityDefinition(header, new OpenApiSecurityScheme
            {
                Name = header,
                Type = SecuritySchemeType.ApiKey,
                In = ParameterLocation.Header,
                Description = "Development only: overrides the Dev:* identity."
            });
            devRequirement[new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = header }
            }] = Array.Empty<string>();
        }

        options.AddSecurityRequirement(devRequirement);
    }

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter a valid JWT bearer token."
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        [new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
            }
        }] = Array.Empty<string>()
    });
});
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddMatrixIdentity(builder.Configuration, builder.Environment);
builder.Services.AddProblemDetails();
var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>();
builder.Services.AddCors(options => options.AddDefaultPolicy(policy => policy
    .WithOrigins(allowedOrigins is { Length: > 0 } ? allowedOrigins : ["http://localhost:5173"])
    .AllowAnyHeader()
    .AllowAnyMethod()
    .WithExposedHeaders("Content-Disposition")));
builder.Services.AddExceptionHandler<MatrixExceptionHandler>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();
app.UseExceptionHandler();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
