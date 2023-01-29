using PersonalFinanceApp.Services.Models;
using PersonalFinanceApp.Services.Models.Queries;

namespace PersonalFinanceApp.Services.Interfaces;
public interface IIncomesService
{
    Task Add(AddIncomeDto dto);
    Task<ICollection<IncomeDto>> GetAllForUser(IncomeExpenseQuery query);
    Task<IncomeDto> GetByIdForUser(int id);
    Task Remove(int id);
    Task Update(AddIncomeDto dto, int id);
}