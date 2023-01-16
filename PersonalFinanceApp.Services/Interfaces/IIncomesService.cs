using PersonalFinanceApp.Services.Models;

namespace PersonalFinanceApp.Services.Interfaces;
public interface IIncomesService
{
    Task Add(AddIncomeDto dto);
    Task<ICollection<IncomeDto>> GetAllForUser();
    Task<IncomeDto> GetByIdForUser(int id);
    Task Remove(int id);
    Task Update(UpdateIncomeDto dto, int id);
}