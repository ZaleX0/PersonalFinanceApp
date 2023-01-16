using Microsoft.EntityFrameworkCore;
using PersonalFinanceApp.Data;
using PersonalFinanceApp.Data.Entities;
using PersonalFinanceApp.Data.Interfaces;
using PersonalFinanceApp.Services.Interfaces;
using PersonalFinanceApp.Services.Models.User;

namespace PersonalFinanceApp.Services.Seeders;

public class FinanceSeeder
{
	private readonly FinanceDbContext _context;
	private readonly IAuthService _authService;

	public FinanceSeeder(FinanceDbContext context, IAuthService authService)
	{
		_context = context;
		_authService = authService;
	}

	public async Task SeedAsync()
	{
		if (!_context.Database.CanConnect())
			return;

		if (!_context.Users.Any())
		{
			await RegisterUserAsync();
        }
	}

	private async Task RegisterUserAsync()
	{
		var registerDto = new RegisterUserDto
		{
			Username = "string",
			Password = "string"
		};
        await _authService.Register(registerDto);
    }
}
