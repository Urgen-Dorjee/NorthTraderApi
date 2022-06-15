using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using NorthTraderAPI;
using NorthTraderAPI.DataServices;
using NorthTraderAPI.Exceptions;
using NorthTraderAPI.NorthwindServices;
using NorthTraderAPI.SwaggerConfigurations;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);
builder.Host.ConfigureLogging(log =>
{
    log.ClearProviders();
    log.AddConsole();
});

builder.Services.AddControllers().AddNewtonsoftJson(opt => opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.OperationFilter<SwaggerDefaultValues>();   
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    opt.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});
builder.Services.AddApplicationInsightsTelemetry();
builder.Services.AddDbContext<NorthwindDbContext>();
builder.Services.AddScoped<ICustomerServices, CustomerServices>();
builder.Services.AddSingleton<IDateTime, MachineDateTime>();
builder.Services.AddScoped<SampleDataSeeder>();
builder.Services.AddApiVersioning();
builder.Services.AddVersionedApiExplorer();
builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

builder.Host.UseSerilog((context, opt) =>
{
    opt.WriteTo.Console();
});


var app = builder.Build();

var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

using var scope = app.Services.CreateScope();
var dbInitializer = scope.ServiceProvider.GetRequiredService<SampleDataSeeder>();
await dbInitializer.SeedAllAsync(cancellationToken: CancellationToken.None);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.DisplayOperationId();
        foreach (var desc in provider.ApiVersionDescriptions)
        {
            c.SwaggerEndpoint($"/swagger/{desc.GroupName}/swagger.json",
                            $"Northwind Trading Co. {desc.GroupName}");
        }
    });
}

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();