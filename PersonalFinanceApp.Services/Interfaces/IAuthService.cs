using PersonalFinanceApp.Data.Entities;
using PersonalFinanceApp.Services.Models;

namespace PersonalFinanceApp.Services.Interfaces;
public interface IAuthService
{
    Task<string> CreateToken(LoginUserDto dto);
    Task Register(RegisterUserDto dto);
}