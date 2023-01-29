using PersonalFinanceApp.Services.Enums;

namespace PersonalFinanceApp.Services.Models;

public class IncomeExpenseDto
{
    public int Id { get; set; }
    public IncomeExpenseType Type { get; set; }
    public int CategoryId { get; set; }
    public string CategoryName { get; set; }
    public decimal Price { get; set; }
    public string? Comment { get; set; }
    public DateTime Date { get; set; }
}
