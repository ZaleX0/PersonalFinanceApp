using PersonalFinanceApp.Data.Interfaces;

namespace PersonalFinanceApp.Data.Entities;

public class RegularIncome : IBaseEntity
{
    public int Id { get; set; }
    public int CategoryId { get; set; }
    public decimal Price { get; set; }
    public string? Comment { get; set; }
    public DateTime LastDateAdded { get; set; }
    public int RepeatingNumberOfDays { get; set; }

    virtual public IncomeCategory Category { get; set; }
}
