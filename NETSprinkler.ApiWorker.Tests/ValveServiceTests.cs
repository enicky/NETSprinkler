using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NETSprinkler.ApiWorker.Business.Drivers;
using NETSprinkler.ApiWorker.Business.Services.Sprinkler;
using NETSprinkler.ApiWorker.Business.Services.Valves;
using NETSprinkler.ApiWorker.Tests.Builders.Sprinkler;
using NETSprinkler.Common.DbContext;
using NETSprinkler.Common.Repositories;
using NETSprinkler.Models.Entity.Valve;

namespace NETSprinkler.ApiWorker.Tests;

public class ValveServiceTests
{
    private readonly SprinklerDbContext _context;
    
    public ValveServiceTests()
    {
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        var optionsBuilder = new DbContextOptionsBuilder<SprinklerDbContext>();
        optionsBuilder.UseSqlite(connection);

        var lf = new LoggerFactory();
        var logger = lf.CreateLogger<SprinklerDbContext>();

        
        _context = new SprinklerDbContext(optionsBuilder.Options, null, logger);
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();
    }

    [Fact]
    public async Task WhenTurningOnValve_ValueOfRecord_GetsUpdated()
    {
        var (sut, cts, dummyGpioDriver) = GetSut();
        var existingValve = SprinklerBuilder.CreateExistingSprinkler();
        await _context.SprinklerValves!.AddAsync(existingValve, cts.Token);
        await _context.SaveChangesAsync(cts.Token);
        await sut.StartAsync(1);


        var valve = await _context.SprinklerValves
            .Include(sprinklerValve => sprinklerValve.Status)
            .FirstAsync(q => q.Id == 1, cts.Token);
        Assert.NotNull(valve);
        Assert.NotNull(valve.Status);
        Assert.True(valve.Status.IsOpen);
        Assert.True(dummyGpioDriver.Pins[existingValve.Id].IsOpen);
    }

    [Fact]
    public async Task WhenTurningOffValue_ValueOfRecord_GetsUpdated()
    {
        var (sut, cts, dummyGpioDriver) = GetSut();
        var existingValve = SprinklerBuilder.CreateExistingSprinkler(isOpen: true);
        await _context.SprinklerValves!.AddAsync(existingValve, cts.Token);
        await _context.SaveChangesAsync(cts.Token);
        await sut.StopAsync(existingValve.Id);


        var valve = await _context.SprinklerValves
            .Include(sprinklerValve => sprinklerValve.Status)
            .FirstAsync(q => q.Id == 1, cts.Token);
        Assert.NotNull(valve);
        Assert.NotNull(valve.Status);
        Assert.False(valve.Status.IsOpen);
        Assert.False(dummyGpioDriver.Pins[existingValve.Id].IsOpen);
    }


    [Fact]
    public async Task GivenWeHaveARecordInTheDb_AndWeWantToQueryItById_WeGetTheRecordFromTheStore()
    {
        var (sut, cts, _) = GetValveSut();
        var entity = SprinklerBuilder.CreateExistingSprinkler();
        await _context.SprinklerValves!.AddAsync(entity, cts.Token);
        await _context.SaveChangesAsync(cts.Token);
        var v = await sut.GetSprinklerValveById(1);
        Assert.NotNull(v);
        Assert.Equal(entity.Name, v.Name);
    }

    private (IValveService, CancellationTokenSource, DummyDriver) GetValveSut()
    {
        var repositoryAsync = new RepositoryAsync<SprinklerValve>(_context);
        ILogger<ValveService> logger = new NullLogger<ValveService>();
        DummyDriver gpioDriver = new DummyDriver();

        IValveService service = new ValveService(logger, repositoryAsync, gpioDriver);
        return (service, new CancellationTokenSource(), gpioDriver);
    }

    private (ISprinklerService, CancellationTokenSource, DummyDriver) GetSut()
    {
        //var q = new MqttService(new MQTTnet.Client.MqttClientOptions() { }, new NullLogger<MqttService>(), )
        //var mqttServiceProvider = new MqttClientServiceProvider();
        var repositoryAsync = new RepositoryAsync<SprinklerValve>(_context);
        ILogger<ValveService> logger = new NullLogger<ValveService>();
        DummyDriver gpioDriver = new DummyDriver();

        IValveService service = new ValveService(logger, repositoryAsync, gpioDriver);
        ISprinklerService sprinklerService =
            new SprinklerService(new NullLogger<SprinklerService>(), service, new UnitOfWork(_context), null);
        return (sprinklerService, new CancellationTokenSource(), gpioDriver);
    }
}