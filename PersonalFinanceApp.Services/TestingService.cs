using Microsoft.EntityFrameworkCore;
using PersonalFinanceApp.Data.Entities;
using PersonalFinanceApp.Data.Interfaces;

namespace PersonalFinanceApp.Services;

public class TestingService
{
	private readonly IFinanceUnitOfWork _unitOfWork;

	public TestingService(IFinanceUnitOfWork unitOfWork)
	{
		_unitOfWork = unitOfWork;
	}

	public List<User> Testing()
	{
		var users = _unitOfWork.Users.Get()
			.Include(u => u.ExpenseCategories).ThenInclude(ec => ec.Expenses)
            .Include(u => u.IncomeCategories).ThenInclude(ic => ic.Incomes)
			.ToList();

		return users;
    }
}
