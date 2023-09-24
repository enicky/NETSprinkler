namespace NETSprinkler.Common.DbContext;

public class UnitOfWork: IUnitOfWork
{
    private readonly SprinklerDbContext _dbContext;
    public UnitOfWork(SprinklerDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _dbContext.SaveChangesAsync(cancellationToken);
    }
}