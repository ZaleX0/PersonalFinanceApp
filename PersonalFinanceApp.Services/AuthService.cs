using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PersonalFinanceApp.Data.Entities;
using PersonalFinanceApp.Data.Interfaces;
using PersonalFinanceApp.Services.Exceptions;
using PersonalFinanceApp.Services.Interfaces;
using PersonalFinanceApp.Services.Models;
using PersonalFinanceApp.Services.Models.User;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PersonalFinanceApp.Services;

public class AuthService : IAuthService
{
	private readonly IMapper _mapper;
	private readonly IFinanceUnitOfWork _unitOfWork;
	private readonly IIncomeCategoriesService _incomeCategoriesService;
	private readonly IExpenseCategoriesService _expenseCategoriesService;
	private readonly IPasswordHasher<User> _passwordHasher;
	private readonly AuthenticationSettings _authenticationSettings;

	public AuthService(
		IMapper mapper,
		IFinanceUnitOfWork unitOfWork,
		IIncomeCategoriesService incomeCategoriesService,
		IExpenseCategoriesService expenseCategoriesService,
		IPasswordHasher<User> passwordHasher,
		AuthenticationSettings authenticationSettings)
	{
		_mapper = mapper;
		_unitOfWork = unitOfWork;
		_incomeCategoriesService = incomeCategoriesService;
		_expenseCategoriesService = expenseCategoriesService;
		_passwordHasher = passwordHasher;
		_authenticationSettings = authenticationSettings;
	}

	public async Task<UserDto> GetUser(LoginUserDto dto)
	{
		var user = await GetUserByUsername(dto.Username);
		VerifyHashedPassword(user, dto.Password);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username)
        };
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);
        var expires = DateTime.Now.AddDays(_authenticationSettings.JwtExpireDays);

        var token = new JwtSecurityToken(
            issuer: _authenticationSettings.JwtIssuer,
            audience: _authenticationSettings.JwtIssuer,
            claims: claims,
            expires: expires,
            signingCredentials: credentials);

        var tokenHandler = new JwtSecurityTokenHandler();

		return new UserDto
		{
			Username = user.Username,
			Token = tokenHandler.WriteToken(token)
		};
    }

    public async Task Register(RegisterUserDto dto)
	{
		var user = _mapper.Map<User>(dto);
		user.Hash = _passwordHasher.HashPassword(user, dto.Password);

		await _unitOfWork.Users.AddAsync(user);

		// Add default categories
		await _incomeCategoriesService.AddDefaultForUser(user);
		await _expenseCategoriesService.AddDefaultForUser(user);

		await _unitOfWork.CommitAsync();
    }

	private async Task<User> GetUserByUsername(string username)
	{
		var user = await _unitOfWork.Users.Get().FirstOrDefaultAsync(u => u.Username == username);
        if (user == null)
            throw new BadRequestException("Invalid username or password");
		return user;
    }

	private void VerifyHashedPassword(User user, string providedPassword)
	{
        var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(user, user.Hash, providedPassword);
        if (passwordVerificationResult == PasswordVerificationResult.Failed)
            throw new BadRequestException("Invalid username or password");
    }
}
