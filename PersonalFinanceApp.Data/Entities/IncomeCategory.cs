using PersonalFinanceApp.Data.Interfaces;

namespace PersonalFinanceApp.Data.Entities;

public class IncomeCategory : IBaseEntity
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Name { get; set; }

    virtual public User User { get; set; }
    virtual public ICollection<Income> Incomes { get; set; }
}
