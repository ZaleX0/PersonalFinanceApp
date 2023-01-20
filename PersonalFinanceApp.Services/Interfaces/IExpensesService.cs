using PersonalFinanceApp.Services.Models;
using PersonalFinanceApp.Services.Models.Queries;

namespace PersonalFinanceApp.Services.Interfaces;
public interface IExpensesService
{
    Task Add(AddExpenseDto dto);
    Task<ICollection<ExpenseDto>> GetAllForUser(IncomeExpenseQuery query);
    Task<ExpenseDto> GetByIdForUser(int id);
    Task Remove(int id);
    Task Update(AddExpenseDto dto, int id);
}