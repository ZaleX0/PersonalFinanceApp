using PersonalFinanceApp.Services.Models;

namespace PersonalFinanceApp.Services.Interfaces;
public interface IRegularIncomesService
{
    Task Add(AddRegularIncomeDto dto);
    Task<ICollection<RegularIncomeDto>> GetAllForUser();
    Task Update(AddRegularIncomeDto dto, int id);
    Task Remove(int id);

    Task HandleAddingIncomes();
}