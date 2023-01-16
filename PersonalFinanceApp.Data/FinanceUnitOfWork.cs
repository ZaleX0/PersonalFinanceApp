using PersonalFinanceApp.Data.Entities;
using PersonalFinanceApp.Data.Interfaces;

namespace PersonalFinanceApp.Data;

public class FinanceUnitOfWork : IFinanceUnitOfWork
{
    private readonly FinanceDbContext _context;

    public FinanceUnitOfWork(FinanceDbContext context,
        IRepository<User> usersRepository,
        IRepository<Income> incomesRepository,
        IRepository<Expense> expensesRepository,
        IRepository<IncomeCategory> incomeCategoriesRepository,
        IRepository<ExpenseCategory> expenseCategoriesRepository)
    {
        _context = context;
        Users = usersRepository;
        Incomes = incomesRepository;
        Expenses = expensesRepository;
        IncomeCategories = incomeCategoriesRepository;
        ExpenseCategories = expenseCategoriesRepository;
    }

    public IRepository<User> Users { get; }
    public IRepository<Income> Incomes { get; }
    public IRepository<Expense> Expenses { get; }
    public IRepository<IncomeCategory> IncomeCategories { get; }
    public IRepository<ExpenseCategory> ExpenseCategories { get; }

public async Task<int> CommitAsync()
    {
        return await _context.SaveChangesAsync();
    }
}
