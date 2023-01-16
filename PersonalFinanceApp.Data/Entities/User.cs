using PersonalFinanceApp.Data.Interfaces;

namespace PersonalFinanceApp.Data.Entities;

public class User : IBaseEntity
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Hash { get; set; }

    virtual public ICollection<IncomeCategory> IncomeCategories { get; set; }
    virtual public ICollection<ExpenseCategory> ExpenseCategories { get; set; }
}
