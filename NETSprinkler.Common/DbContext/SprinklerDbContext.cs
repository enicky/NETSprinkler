using Microsoft.EntityFrameworkCore;
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
    public DbSet<Schedule> Schedules { get; set; }
    public DbSet<ValveStatus> ValveStatus { get; set; }
    public DbSet<SprinklerValve> SprinklerValves { get; set; }
    
    public SprinklerDbContext(DbContextOptions<SprinklerDbContext> o, IOptions<DbConfigurationOptions>? dBConfiguration) : base(o){
        if(dBConfiguration != null)
            _dBConfiguration = dBConfiguration.Value;
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if(_dBConfiguration != null)
            optionsBuilder.UseSqlServer(_dBConfiguration.ConnectionString);
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