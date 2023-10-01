using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NETSprinkler.Business.DbContext.Configuration;
using NETSprinkler.Models.Entity;
using NETSprinkler.Models.Entity.Schedule;
using NETSprinkler.Models.Entity.Status;
using NETSprinkler.Models.Entity.Valve;

namespace NETSprinkler.Common.DbContext;

public class SprinklerDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    private readonly DbConfigurationOptions? _dBConfiguration = null;
    public DbSet<Schedule>? Schedules { get; set; }
    public DbSet<ValveStatus>? ValveStatus { get; set; }
    public DbSet<SprinklerValve>? SprinklerValves { get; set; }

    private ILogger<SprinklerDbContext> _logger { get; set; }

    public SprinklerDbContext(DbContextOptions<SprinklerDbContext> o, IOptions<DbConfigurationOptions>? dBConfiguration ,
        ILogger<SprinklerDbContext> logger
                        /*ILogger<SprinklerDbContext> logger*/) : base(o)
    {
        _logger = logger;
        if(dBConfiguration != null)
            _dBConfiguration = dBConfiguration.Value;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if(_dBConfiguration != null) {
            var host = _dBConfiguration.Host;
            var username = _dBConfiguration.Username;
            var password = _dBConfiguration.Password;
            var connectionString = _dBConfiguration.ConnectionString;
            
            connectionString = connectionString.Replace("{host}", host).Replace("{username}", username).Replace("{password}", password);
            optionsBuilder.UseSqlServer(connectionString);

        }
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Schedule>()
            .Property(e => e.DaysToRun)
            .HasConversion(v => string.Join(",", v.Select(e => e.ToString("D")).ToArray()),
                v => v.Split(new[] { ',' })
                    .Select(Enum.Parse<DayReference>)
                    .ToList()
                );
            
        base.OnModelCreating(modelBuilder);
    }
}