using PersonalFinanceApp.Data.Enums;

namespace PersonalFinanceApp.Data.Entities;

public class RegularIncome
{
    public int Id { get; set; }
    public int CategoryId { get; set; }
    public decimal Price { get; set; }
    public string? Comment { get; set; }
    public RegularPeriodType RegularPeriodType { get; set; }
    public DateTime DateAddedInCurrentPeriod { get; set; }
    public int NumberOfDays { get; set; }

    virtual public IncomeCategory Category { get; set; }
}
