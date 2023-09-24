using System.Linq.Expressions;

namespace NETSprinkler.Business.Services;

public interface IServiceAsync<TEntity, TDto>
{
    IEnumerable<TDto> GetAll(Expression<Func<TDto, bool>>? expression = null);
    Task AddAsync(TDto entity);
    Task DeleteAsync(int id);
}