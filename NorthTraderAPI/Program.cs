using System.Reflection;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using NorthTraderAPI;
using NorthTraderAPI.DataServices;
using NorthTraderAPI.Exceptions;
using NorthTraderAPI.NorthwindServices;

var builder = WebApplication.CreateBuilder(args);
builder.Host.ConfigureLogging(log =>
{
    log.ClearProviders();
    log.AddConsole();
});

builder.Services.AddControllers()
    .AddNewtonsoftJson(opt
        => opt.SerializerSettings.ReferenceLoopHandling
            = ReferenceLoopHandling.Ignore);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "NorthwindTraderAPI",
        Description = "Northwind Trading Co.",
        Contact = new OpenApiContact
        {
            Name = "Urgen Dorjee",
            Email = "urgen0240@gmail.com"
        },
        License = new OpenApiLicense
        {
            Name = "Northwind Trading Registered License"
        }
    });
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    opt.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});
builder.Services.AddApplicationInsightsTelemetry();
builder.Services.AddDbContext<NorthwindDbContext>();
builder.Services.AddScoped<ICustomerServices, CustomerServices>();
builder.Services.AddSingleton<IDateTime, MachineDateTime>();
builder.Services.AddScoped<SampleDataSeeder>();



var app = builder.Build();


using var scope = app.Services.CreateScope();
var dbInitializer = scope.ServiceProvider.GetRequiredService<SampleDataSeeder>();
await dbInitializer.SeedAllAsync(cancellationToken: CancellationToken.None);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();