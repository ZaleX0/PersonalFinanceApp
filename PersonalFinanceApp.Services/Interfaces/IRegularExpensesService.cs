using PersonalFinanceApp.Services.Models;

namespace PersonalFinanceApp.Services.Interfaces;
public interface IRegularExpensesService
{
    Task Add(AddRegularExpenseDto dto);
    Task<ICollection<RegularExpenseDto>> GetAllForUser();
    Task Update(AddRegularExpenseDto dto, int id);
    Task Remove(int id);

    Task HandleAddingExpenses();
}