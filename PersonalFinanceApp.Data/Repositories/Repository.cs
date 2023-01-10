using Microsoft.EntityFrameworkCore;
using PersonalFinanceApp.Data.Interfaces;

namespace PersonalFinanceApp.Data.Repositories;

public class Repository<T> : IRepository<T> where T : class, IEntity
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
