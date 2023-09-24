using System.Linq.Expressions;
using AutoMapper;
using NETSprinkler.Business.Repositories;
using NETSprinkler.Contracts.Entity;
using NETSprinkler.Models.Entity;

namespace NETSprinkler.Business.Services;

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

    public Task AddAsync(TDto entity)
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