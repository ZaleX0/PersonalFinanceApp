using System.Security.Claims;

namespace PersonalFinanceApp.Services.Interfaces;
public interface IUserContextService
{
    int? GetUserId { get; }
    ClaimsPrincipal? User { get; }
}