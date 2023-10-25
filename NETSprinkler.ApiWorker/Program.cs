using NETSprinkler.Business.DbContext.Configuration;
using NETSprinkler.Common.DbContext;
using Hangfire;
using Hangfire.Console.Extensions;
using NETSprinkler.ApiWorker.Business.Helpers;
using Serilog;
using Serilog.Events;
using Serilog.Extensions.Logging;
using NETSprinkler.ApiWorker;
using NETSprinkler.Common.Config.Mqtt;
using NETSprinkler.Common.Config.Gpio;
using NETSprinkler.ApiWorker.Business.Extensions;
using AutoMapper.Extensions.ExpressionMapping;
using NETSprinkler.ApiWorker.Business.Automapper;

void SetConfiguration(IConfigurationBuilder builder)
    => builder.AddUserSecrets<Program>()
        .AddEnvironmentVariables()
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", true);

var configurationBuilder = new ConfigurationBuilder();
SetConfiguration(configurationBuilder);
var configuration = configurationBuilder.Build();




var builder = WebApplication.CreateBuilder(args);
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5000);
});
builder.Configuration.AddConfiguration(configuration);

//builder.Configuration.AddJsonFile("appsettings.json", optional:true, reloadOnChange:true)
//    .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", true)
//    .AddEnvironmentVariables();
// Add services to the container.
using var log = new LoggerConfiguration() 
    .ReadFrom.Configuration(configuration)
    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning) 
    .CreateLogger();
Log.Logger = log; //new 
//builder.Services.AddSingleton<Serilog.ILogger>(log);


builder.Services.AddLogging(lb => lb.SetMinimumLevel(LogLevel.Warning).AddConsole().AddSerilog());
builder.Logging.AddFilter<SerilogLoggerProvider>(null, LogLevel.Warning);
builder.Services.Configure<DbConfigurationOptions>(builder.Configuration.GetSection("DBConfiguration"));
builder.Services.Configure<MqttConfigurationOptions>(builder.Configuration.GetSection("Mqtt"));
builder.Services.Configure<GpioConfigurationOptions>(builder.Configuration.GetSection("GPIO"));



//var mqttService = new MqttService(mqttConfiguration!.Value, builder.Services.BuildServiceProvider().GetService<ILogger<MqttService>>());
//await mqttService.StartMqttClient();

builder.Services.AddMqttClientHostedService();

//builder.Services.AddSingleton<IMqttService, MqttService>();

builder.Services.AddDbContext<SprinklerDbContext>();
builder.Services.AddEntityFrameworkSqlServer();
builder.Services.AddNetSprinkler();
builder.Services.AddControllers();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var _dBConfiguration = builder.Configuration.GetSection("DBConfiguration");
var host = _dBConfiguration["host"];
var username = _dBConfiguration["Username"];
var password = _dBConfiguration["Password"];
var connectionString = _dBConfiguration["ConnectionString"];
 connectionString = connectionString!.Replace("{host}", host).Replace("{username}", username).Replace("{password}", password);


builder.Services.AddHangfire(configuration => configuration
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseSerilogLogProvider()
    .UseSqlServerStorage(connectionString));

builder.Services.AddHangfireConsoleExtensions();
builder.Services.AddHangfireServer();
builder.Services.AddSerilog();
builder.Services.AddAutoMapper(c =>
{
    c.AddExpressionMapping();
}, typeof(MappingProfile).Assembly);



var app = builder.Build();
var options = new DashboardOptions()
{
    Authorization = new[] { new DashboardNoAuthorizationFilter() }

};
app.UseHangfireDashboard(options: options);

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();