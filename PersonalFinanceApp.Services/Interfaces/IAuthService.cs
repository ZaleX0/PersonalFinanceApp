using PersonalFinanceApp.Data.Entities;
using PersonalFinanceApp.Services.Models.User;

namespace PersonalFinanceApp.Services.Interfaces;
public interface IAuthService
{
    Task<string> CreateToken(LoginUserDto dto);
    Task Register(RegisterUserDto dto);
}