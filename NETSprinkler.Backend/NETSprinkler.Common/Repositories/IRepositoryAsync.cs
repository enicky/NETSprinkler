using System.Linq.Expressions;
using NETSprinkler.Models.Entity;

namespace NETSprinkler.Common.Repositories;

public interface IRepositoryAsync<T> where T: Entity
{
    IQueryable<T> Entities { get; }
    IQueryable<T> GetAll(Expression<Func<T, bool>>? expression = null);
    IQueryable<T> GetAllWithInclude(Expression<Func<T, Entity>> includeExpression);
    Task<T> AddAsync(T entity);
    Task DeleteAsync(T entity);
    Task<T?> GetById(int id);
    Task<T?> GetByIdWithInclude(int id, Expression<Func<T, Entity>> expression);
}