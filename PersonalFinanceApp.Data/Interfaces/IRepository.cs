using System.Linq.Expressions;

namespace PersonalFinanceApp.Data.Interfaces;

public interface IRepository<T>
{
    Task AddAsync(T entity);
    Task<T?> GetByIdAsync(int id);
    IQueryable<T> Get();
    void Update(T entity);
    void Remove(T entity);
}
