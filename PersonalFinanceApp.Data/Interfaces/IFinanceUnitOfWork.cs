using PersonalFinanceApp.Data.Entities;

namespace PersonalFinanceApp.Data.Interfaces;

public interface IFinanceUnitOfWork
{
    IRepository<User> Users { get; }
    IRepository<Income> Incomes { get; }
    IRepository<Expense> Expenses { get; }
    IRepository<IncomeCategory> IncomeCategories { get; }
    IRepository<ExpenseCategory> ExpenseCategories { get; }
    IRepository<RegularIncome> RegularIncomes { get; }
    IRepository<RegularExpense> RegularExpenses { get; }

    Task<int> CommitAsync();
}
