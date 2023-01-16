using PersonalFinanceApp.Data.Entities;
using PersonalFinanceApp.Services.Models;

namespace PersonalFinanceApp.Services.Interfaces;
public interface IExpenseCategoriesService
{
    Task Add(ExpenseCategoryDto dto);
    Task<ICollection<ExpenseCategoryDto>> GetForUser();
    Task Remove(int id);
    Task Update(ExpenseCategoryDto dto, int id);

    Task AddDefaultForUser(User user);
}