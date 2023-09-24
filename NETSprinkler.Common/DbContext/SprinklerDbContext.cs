using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NETSprinkler.Business.DbContext.Configuration;
using NETSprinkler.Models.Entity.Schedule;


namespace NETSprinkler.Business.DbContext;

public class SprinklerDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    private readonly DbConfigurationOptions _dBConfiguration;
    public DbSet<Schedule> Schedules { get; set; }
    
    public SprinklerDbContext(IOptions<DbConfigurationOptions> dBConfiguration)
    {
        _dBConfiguration = dBConfiguration.Value;
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(_dBConfiguration.ConnectionString);
        base.OnConfiguring(optionsBuilder);
    }
}