using System.Linq.Expressions;
using NETSprinkler.Models.Entity;

namespace NETSprinkler.Business.Repositories;

public interface IRepositoryAsync<T> where T: Entity
{
    IQueryable<T> Entities { get; }
    IQueryable<T> GetAll(Expression<Func<T, bool>>? expression = null);
    Task AddAsync(T entity);
    Task DeleteAsync(T entity);
    Task<T?> GetById(int id);
}