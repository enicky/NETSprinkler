using System.Linq.Expressions;
using AutoMapper;
using NETSprinkler.Common.Repositories;
using NETSprinkler.Contracts.Entity;
using NETSprinkler.Models.Entity;

namespace NETSprinkler.Common.Services;

public class ServiceAsync<TEntity> : IServiceAsync<TEntity> where TEntity : Entity
{
    private readonly IRepositoryAsync<TEntity> _repositoryAsync;

    protected ServiceAsync(IRepositoryAsync<TEntity> repositoryAsync)
    {
        _repositoryAsync = repositoryAsync;
    }
    public Task<TEntity?> GetById(int id)
    {
        return _repositoryAsync.GetById(id);
    }

    public Task<TEntity?> GetByIdWithInclude(int id, Expression<Func< TEntity, Entity>> expression)
    {
        var x = _repositoryAsync.GetByIdWithInclude(id: id, expression: expression);
        return x;
    }
}

public class ServiceAsync<TEntity, TDto>: IServiceAsync<TEntity, TDto>
    where TEntity: Entity where TDto: EntityDto
{
    private readonly IRepositoryAsync<TEntity> _repositoryAsync;
    private readonly IMapper _mapper;

    protected ServiceAsync(IRepositoryAsync<TEntity> repositoryAsync, IMapper mapper)
    {
        _repositoryAsync = repositoryAsync;
        _mapper = mapper;
    }
    
    public IEnumerable<TDto> GetAll(Expression<Func<TDto, bool>>? expression = null)
    {
        var predicate = _mapper.Map<Expression<Func<TEntity, bool>>>(expression);
        return _repositoryAsync.GetAll(predicate).AsEnumerable().Select(_mapper.Map<TDto>).ToList();
    }

    public Task<TEntity> AddAsync(TDto entity)
    {
        var e = _mapper.Map<TEntity>(entity);
        return _repositoryAsync.AddAsync(e);
    }

    public async Task DeleteAsync(int id)
    {
        var e = await _repositoryAsync.GetById(id);
        if (e == null) return;
        await _repositoryAsync.DeleteAsync(e);
    }
}