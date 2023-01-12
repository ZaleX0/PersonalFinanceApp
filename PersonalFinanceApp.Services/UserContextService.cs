using Microsoft.AspNetCore.Http;
using PersonalFinanceApp.Services.Interfaces;
using System.Security.Claims;

namespace PersonalFinanceApp.Services;

public class UserContextService : IUserContextService
{
	private readonly IHttpContextAccessor _httpContextAccessor;

	public UserContextService(IHttpContextAccessor httpContextAccessor)
	{
		_httpContextAccessor = httpContextAccessor;
	}

	public ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;

	public int? GetUserId => User != null
		? int.Parse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value)
		: null;
}
