using Application.Services.Implement;
using Application.Services.Interface;
using Infrastructure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using WebAPI.Authentication;
using WebAPI.ExceptionHandling;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddScoped<IAcademicYearService, AcademicYearService>();
builder.Services.AddScoped<IProvinceCatalogService, ProvinceCatalogService>();
builder.Services.AddScoped<IProvinceSyncService, ProvinceSyncService>();
builder.Services.AddExceptionHandler<ApiExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
        policy.WithOrigins("http://localhost:5173", "http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod());
});

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddAuthentication("DevBearer")
        .AddScheme<AuthenticationSchemeOptions, DevAuthenticationHandler>("DevBearer", _ => { });
}
else
{
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();
}

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("OperationalAdmin", policy =>
        policy.RequireAuthenticatedUser().RequireAssertion(context =>
            context.User.IsInRole("OperationalAdmin") ||
            context.User.HasClaim("permission", "academic_calendar.manage")));
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowFrontend");
app.UseExceptionHandler();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

public partial class Program;
