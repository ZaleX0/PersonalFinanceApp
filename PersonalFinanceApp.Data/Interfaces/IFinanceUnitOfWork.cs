using PersonalFinanceApp.Data.Entities;

namespace PersonalFinanceApp.Data.Interfaces;

public interface IFinanceUnitOfWork
{
    IRepository<User> User { get; }
    IRepository<IncomeCategory> IncomeCategories { get; }
    IRepository<ExpenseCategory> ExpenseCategories { get; }
    IRepository<Income> Incomes { get; }
    IRepository<Expense> Expenses { get; }
}
