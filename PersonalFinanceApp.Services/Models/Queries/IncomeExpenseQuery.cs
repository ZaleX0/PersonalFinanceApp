namespace PersonalFinanceApp.Services.Models.Queries;

public class IncomeExpenseQuery
{
    public string? Search { get; set; }
    public int? IncomeCategoryId { get; set; }
    public int? ExpenseCategoryId { get; set; }
    public DateTime? DateFrom { get; set; }
    public DateTime? DateTo { get; set; }    
}
