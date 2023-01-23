using PersonalFinanceApp.Services.Models;
using PersonalFinanceApp.Services.Models.Queries;

namespace PersonalFinanceApp.Services.Interfaces;
public interface IIncomeExpenseService
{
    Task<ICollection<IncomeExpenseDto>> GetIncomeExpenseData(IncomeExpenseQuery query);
}