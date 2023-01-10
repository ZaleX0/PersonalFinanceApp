using PersonalFinanceApp.Data.Interfaces;

namespace PersonalFinanceApp.Data.Entities;

public class Income : IEntity
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int CategoryId { get; set; }
    public decimal Price { get; set; }
    public string? Comment { get; set; }
    public DateTime Date { get; set; }

    virtual public User User { get; set; }
    virtual public IncomeCategory Category { get; set; }
}
