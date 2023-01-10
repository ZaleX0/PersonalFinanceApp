using PersonalFinanceApp.Data.Entities;
using PersonalFinanceApp.Data.Interfaces;

namespace PersonalFinanceApp.Data;

public class FinanceUnitOfWork : IFinanceUnitOfWork
{
    private readonly FinanceDbContext _context;

    public FinanceUnitOfWork(FinanceDbContext context,
        IRepository<IncomeCategory> incomeCategoriesRepository,
        IRepository<ExpenseCategory> expenseCategoriesRepository)
    {
        _context = context;
        IncomeCategories = incomeCategoriesRepository;
        ExpenseCategories = expenseCategoriesRepository;
    }

    public IRepository<IncomeCategory> IncomeCategories { get; }
    public IRepository<ExpenseCategory> ExpenseCategories { get; }

    public async Task<int> CommitAsync()
    {
        return await _context.SaveChangesAsync();
    }
}
