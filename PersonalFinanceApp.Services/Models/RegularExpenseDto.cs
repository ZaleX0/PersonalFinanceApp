﻿namespace PersonalFinanceApp.Services.Models;

public class RegularExpenseDto
{
    public int Id { get; set; }
    public int CategoryId { get; set; }
    public string CategoryName { get; set; }
    public decimal Price { get; set; }
    public string? Comment { get; set; }
    public DateTime LastDateAdded { get; set; }
    public int RepeatingNumberOfDays { get; set; }
}
