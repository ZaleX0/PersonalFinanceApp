﻿using FluentValidation;
using PersonalFinanceApp.Data;
using PersonalFinanceApp.Data.Entities;

namespace PersonalFinanceApp.Services.Models.Validators;

public class RegisterUserDtoValidator : AbstractValidator<RegisterUserDto>
{
	public RegisterUserDtoValidator(FinanceDbContext dbContext)
	{
		RuleFor(u => u.Username).NotEmpty();
		RuleFor(u => u.Password).MinimumLength(6);
		RuleFor(u => u.Username).Custom((value, context) =>
		{
			var usernameInUse = dbContext.Users.Any(u => u.Username.ToLower() == value.ToLower());
			if (usernameInUse)
			{
				context.AddFailure(nameof(User.Username), "That username is taken");
			}
		});
	}
}