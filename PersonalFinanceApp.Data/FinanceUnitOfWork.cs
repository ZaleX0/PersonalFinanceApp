using PersonalFinanceApp.Data.Entities;
using PersonalFinanceApp.Data.Interfaces;

namespace PersonalFinanceApp.Data;

public class FinanceUnitOfWork : IFinanceUnitOfWork
{
    private readonly FinanceDbContext _context;

    public FinanceUnitOfWork(FinanceDbContext context,
        IRepository<User> users,
        IRepository<Income> incomes,
        IRepository<Expense> expenses,
        IRepository<IncomeCategory> incomeCategories,
        IRepository<ExpenseCategory> expenseCategories,
        IRepository<RegularIncome> regularIncomes,
        IRepository<RegularExpense> regularExpenses)
    {
        _context = context;
        Users = users;
        Incomes = incomes;
        Expenses = expenses;
        IncomeCategories = incomeCategories;
        ExpenseCategories = expenseCategories;
        RegularIncomes = regularIncomes;
        RegularExpenses = regularExpenses;
    }

    public IRepository<User> Users { get; }
    public IRepository<Income> Incomes { get; }
    public IRepository<Expense> Expenses { get; }
    public IRepository<IncomeCategory> IncomeCategories { get; }
    public IRepository<ExpenseCategory> ExpenseCategories { get; }
    public IRepository<RegularIncome> RegularIncomes { get; }
    public IRepository<RegularExpense> RegularExpenses { get; }

    public async Task<int> CommitAsync()
    {
        return await _context.SaveChangesAsync();
    }
}
