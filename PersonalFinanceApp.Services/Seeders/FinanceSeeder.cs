using PersonalFinanceApp.Data;
using PersonalFinanceApp.Data.Entities;
using PersonalFinanceApp.Data.Interfaces;

namespace PersonalFinanceApp.Services.Seeders;

public class FinanceSeeder
{
	private readonly FinanceDbContext _context;

	public FinanceSeeder(FinanceDbContext context)
	{
		_context = context;
	}

	public void Seed()
	{
		// TODO: Seed
		return;

		if (!_context.Database.CanConnect())
			return;

		if (!_context.Users.Any())
		{
			var user = GetUser();
            _context.Users.Add(user);

			var ic = new IncomeCategory
			{
				User = user,
				Name = "Income Category"
			};
			var ec = new ExpenseCategory
			{
				User = user,
				Name = "Expense Category"
			};
            _context.IncomeCategories.Add(ic);
            _context.ExpenseCategories.Add(ec);

			_context.Incomes.Add(new Income
			{
				Category = ic,
				Price = 1.00m,
				Date = DateTime.Now,
				Comment = "Income"
			});

            _context.Expenses.Add(new Expense
            {
                Category = ec,
                Price = 1.00m,
                Date = DateTime.Now,
                Comment = "Expense"
            });

			_context.SaveChanges();
        }

	}

	private User GetUser()
	{
		return new User
		{
			Username = "user",
			//Hash = "user"
        };
    }
}
