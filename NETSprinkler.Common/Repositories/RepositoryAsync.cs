using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using NETSprinkler.Business.DbContext;
using NETSprinkler.Models.Entity;

namespace NETSprinkler.Business.Repositories;

public class RepositoryAsync<T>: IRepositoryAsync<T> where T: Entity
{
    private readonly SprinklerDbContext _dbContext;
    public IQueryable<T> Entities => _dbContext.Set<T>();
    
    public RepositoryAsync(SprinklerDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public IQueryable<T> GetAll(Expression<Func<T, bool>>? expression = null)
    {
        var result = _dbContext.Set<T>().AsNoTracking();
        if (expression != null)
            result = result.Where(expression);
        return result;
    }

    public async Task AddAsync(T entity)
    {
        await _dbContext.Set<T>().AddAsync(entity);
    }

    public Task DeleteAsync(T entity)
    {
         _dbContext.Set<T>().Remove(entity);
         return Task.CompletedTask;
    }

    public async Task<T?> GetById(int id)
    {
        return await _dbContext.Set<T>().FindAsync(id);
    }
}