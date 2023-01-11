using PersonalFinanceApp.Data.Interfaces;

namespace PersonalFinanceApp.Services;

public class UserService
{
	private readonly IFinanceUnitOfWork _unitOfWork;

	public UserService(IFinanceUnitOfWork unitOfWork)
	{
		_unitOfWork = unitOfWork;
	}

}
