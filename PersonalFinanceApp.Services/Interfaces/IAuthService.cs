using PersonalFinanceApp.Data.Entities;
using PersonalFinanceApp.Services.Models;
using PersonalFinanceApp.Services.Models.User;

namespace PersonalFinanceApp.Services.Interfaces;
public interface IAuthService
{
    Task<UserDto> GetUser(LoginUserDto dto);
    Task Register(RegisterUserDto dto);
}