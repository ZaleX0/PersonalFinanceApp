using PersonalFinanceApp.Data.Entities;

namespace PersonalFinanceApp.Data.Interfaces;

public interface IFinanceUnitOfWork
{
    IRepository<IncomeCategory> IncomeCategories { get; }
    IRepository<ExpenseCategory> ExpenseCategories { get; }
}
