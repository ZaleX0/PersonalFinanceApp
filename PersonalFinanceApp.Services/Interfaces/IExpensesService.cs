using PersonalFinanceApp.Services.Models;

namespace PersonalFinanceApp.Services.Interfaces;
public interface IExpensesService
{
    Task Add(AddExpenseDto dto);
    Task<ICollection<ExpenseDto>> GetAllForUser();
    Task<ExpenseDto> GetByIdForUser(int id);
    Task Remove(int id);
    Task Update(UpdateExpenseDto dto, int id);
}