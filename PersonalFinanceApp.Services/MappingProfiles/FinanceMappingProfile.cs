using AutoMapper;
using PersonalFinanceApp.Data.Entities;
using PersonalFinanceApp.Services.Models;
using PersonalFinanceApp.Services.Models.User;

namespace PersonalFinanceApp.Services.MappingProfiles;

public class FinanceMappingProfile : Profile
{
    public FinanceMappingProfile()
    {
        CreateMap<RegisterUserDto, User>();
        CreateMap<IncomeCategory, IncomeCategoryDto>();
        CreateMap<ExpenseCategory, ExpenseCategoryDto>();

        CreateMap<Income, IncomeDto>();
        CreateMap<AddIncomeDto, Income>();

        CreateMap<Expense, ExpenseDto>();
        CreateMap<AddExpenseDto, Expense>();
    }
}
