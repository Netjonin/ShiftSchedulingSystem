using Contracts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Repository;

public class RepositoryBase<T> : IRepositoryBase<T> where T : class
{
    private readonly ApplicationContext _ctx;
    public RepositoryBase(ApplicationContext ctx)
    => _ctx = ctx ?? throw new ArgumentNullException(nameof(ctx));

    public void Create(T entity) => _ctx.Set<T>().Add(entity);

    public void Delete(T entity) => _ctx.Set<T>().Remove(entity);

    public IQueryable<T> FindAll(bool trackChanges) =>
        !trackChanges ? _ctx.Set<T>().AsNoTracking() : _ctx.Set<T>();

    public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges) =>
    !trackChanges ? _ctx.Set<T>().Where(expression).AsNoTracking() : _ctx.Set<T>().Where(expression);

    public void Update(T entity) => _ctx.Set<T>().Update(entity);
}

