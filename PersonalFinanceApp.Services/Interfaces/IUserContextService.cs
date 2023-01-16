using System.Security.Claims;

namespace PersonalFinanceApp.Services.Interfaces;
public interface IUserContextService
{
    ClaimsPrincipal? User { get; }
    int? UserId { get; }
    int TryGetUserId();
}