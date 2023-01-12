using AutoMapper;
using PersonalFinanceApp.Data.Entities;
using PersonalFinanceApp.Services.Models;

namespace PersonalFinanceApp.Services.MappingProfiles;

public class FinanceMappingProfile : Profile
{
    public FinanceMappingProfile()
    {
        CreateMap<RegisterUserDto, User>();
    }
}
