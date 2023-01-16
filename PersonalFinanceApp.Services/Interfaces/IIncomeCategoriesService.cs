using PersonalFinanceApp.Data.Entities;
using PersonalFinanceApp.Services.Models;

namespace PersonalFinanceApp.Services.Interfaces;
public interface IIncomeCategoriesService
{
    Task Add(IncomeCategoryDto dto);
    Task<ICollection<IncomeCategoryDto>> GetAllForUser();
    Task Remove(int id);
    Task Update(IncomeCategoryDto dto, int id);

    Task AddDefaultForUser(User user);
}