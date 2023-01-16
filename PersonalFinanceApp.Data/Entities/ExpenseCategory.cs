using PersonalFinanceApp.Data.Interfaces;

namespace PersonalFinanceApp.Data.Entities;

public class ExpenseCategory : IBaseEntity
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Name { get; set; }

    virtual public User User { get; set; }
    virtual public ICollection<Expense> Expenses { get; set; }
    virtual public ICollection<RegularExpense> RegularExpenses { get; set; }
}
