using PersonalFinanceApp.Data.Interfaces;

namespace PersonalFinanceApp.Data.Entities;

public class ExpenseCategory : IEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
}
