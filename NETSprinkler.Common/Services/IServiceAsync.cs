using System.Linq.Expressions;
using NETSprinkler.Models.Entity;

namespace NETSprinkler.Common.Services;

public interface IServiceAsync<TEntity, TDto>
{
    IEnumerable<TDto> GetAll(Expression<Func<TDto, bool>>? expression = null);
    Task<TEntity> AddAsync(TDto entity);
    Task DeleteAsync(int id);
}

public interface IServiceAsync<TEntity>
{
    Task<TEntity?> GetById(int id);
    Task<TEntity?> GetByIdWithInclude(int id, Expression<Func< TEntity, Entity>> expression);
}