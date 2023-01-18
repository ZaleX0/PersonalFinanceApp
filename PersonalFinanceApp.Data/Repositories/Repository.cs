using Microsoft.EntityFrameworkCore;
using PersonalFinanceApp.Data.Interfaces;
using System.Linq.Expressions;

namespace PersonalFinanceApp.Data.Repositories;

public class Repository<T> : IRepository<T> where T : class, IBaseEntity
{
    protected readonly FinanceDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public Repository(FinanceDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public async Task AddRangeAsync(ICollection<T> entities)
    {
        await _dbSet.AddRangeAsync(entities);
    }

    public IQueryable<T> Get()
    {
        return _dbSet;
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await _dbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(entity => entity.Id.Equals(id));
    }

    public void Remove(T entity)
    {
        _dbSet.Remove(entity);
    }

    public void Update(T entity)
    {
        _dbSet.Update(entity);
    }
}
