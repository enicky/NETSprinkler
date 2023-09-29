using NETSprinkler.Business.DbContext.Configuration;
using NETSprinkler.Common.DbContext;
using Hangfire;
using Hangfire.Console.Extensions;
using NETSprinkler.ApiWorker.Business.Helpers;
using Serilog;
using Serilog.Events;
using Serilog.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.json", optional:true, reloadOnChange:true)
    .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", true)
    .AddEnvironmentVariables();
// Add services to the container.
using var log = new LoggerConfiguration() 
    .ReadFrom.Configuration(builder.Configuration)
    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning) 
    .CreateLogger();
Log.Logger = log; //new 
//builder.Services.AddSingleton<Serilog.ILogger>(log);

builder.Services.AddLogging(lb => lb.SetMinimumLevel(LogLevel.Warning).AddConsole().AddSerilog());
builder.Logging.AddFilter<SerilogLoggerProvider>(null, LogLevel.Warning);
builder.Services.Configure<DbConfigurationOptions>(builder.Configuration.GetSection("DBConfiguration"));

builder.Services.AddDbContext<SprinklerDbContext>();
builder.Services.AddEntityFrameworkSqlServer();
builder.Services.AddNetSprinkler();
builder.Services.AddControllers();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



builder.Services.AddHangfire(configuration => configuration
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseSerilogLogProvider()
    .UseSqlServerStorage(builder.Configuration.GetSection("DBConfiguration")["ConnectionString"]));

builder.Services.AddHangfireConsoleExtensions();
builder.Services.AddHangfireServer();
builder.Services.AddSerilog();

var app = builder.Build();
app.UseHangfireDashboard();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();