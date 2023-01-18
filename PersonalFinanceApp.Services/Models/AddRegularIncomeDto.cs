namespace PersonalFinanceApp.Services.Models;

public class AddRegularIncomeDto
{
    public int CategoryId { get; set; }
    public decimal Price { get; set; }
    public string? Comment { get; set; }
    public DateTime LastDateAdded { get; set; } = DateTime.Today;
    public int RepeatingNumberOfDays { get; set; } = 30;
}
