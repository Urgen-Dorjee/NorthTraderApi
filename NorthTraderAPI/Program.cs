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
        =>opt.SerializerSettings.ReferenceLoopHandling
            =ReferenceLoopHandling.Ignore);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplicationInsightsTelemetry();
builder.Services.AddDbContext<NorthwindDbContext>();
builder.Services.AddScoped<ICustomerServices, CustomerServices>();
builder.Services.AddSingleton<IDateTime, MachineDateTime>();
builder.Services.AddScoped<SampleDataSeeder>();



var app = builder.Build();

//var logger = app.Services.GetRequiredService<ILogger>();

using var scope = app.Services.CreateScope();
var dbInitializer = scope.ServiceProvider.GetRequiredService<SampleDataSeeder>();
await dbInitializer.SeedAllAsync(cancellationToken: CancellationToken.None);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();