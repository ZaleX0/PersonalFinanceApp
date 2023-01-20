using FluentValidation;
using PersonalFinanceApp.Data;
using PersonalFinanceApp.Services.Models.User;

namespace PersonalFinanceApp.Services.Models.Validators;

public class RegisterUserDtoValidator : AbstractValidator<RegisterUserDto>
{
	public RegisterUserDtoValidator(FinanceDbContext dbContext)
	{
		RuleFor(u => u.Username).MinimumLength(3).WithMessage("Username must be at least 3 characters");
		RuleFor(u => u.Password).MinimumLength(6).WithMessage("Password must be at least 6 characters"); ;
		RuleFor(u => u.Username).Custom((value, context) =>
		{
			var usernameInUse = dbContext.Users.Any(u => u.Username.ToLower() == value.ToLower());
			if (usernameInUse)
				context.AddFailure("Username", "Username already taken");
		});
	}
}
