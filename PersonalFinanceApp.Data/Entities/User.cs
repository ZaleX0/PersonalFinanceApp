using PersonalFinanceApp.Data.Interfaces;

namespace PersonalFinanceApp.Data.Entities;

public class User : IEntity
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Hash { get; set; }

    virtual public ICollection<Income> Incomes { get; set; }
    virtual public ICollection<Expense> Expenses { get; set; }
}
