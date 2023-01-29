using AutoMapper;
using PersonalFinanceApp.Data.Interfaces;
using PersonalFinanceApp.Services.Interfaces;
using PersonalFinanceApp.Services.Models;
using PersonalFinanceApp.Services.Models.Queries;

namespace PersonalFinanceApp.Services;

public class IncomeExpenseService : IIncomeExpenseService
{
	private readonly IMapper _mapper;
	private readonly IFinanceUnitOfWork _unitOfWork;
	private readonly IIncomesService _incomesService;
	private readonly IExpensesService _expensesService;
	private readonly IRegularIncomesService _regularIncomesService;
	private readonly IRegularExpensesService _regularExpensesService;

	public IncomeExpenseService(
		IMapper mapper,
		IFinanceUnitOfWork unitOfWork,
		IIncomesService incomesService,
		IExpensesService expensesService,
        IRegularIncomesService regularIncomesService,
        IRegularExpensesService regularExpensesService)
	{
		_mapper = mapper;
		_unitOfWork = unitOfWork;
		_incomesService = incomesService;
		_expensesService = expensesService;
		_regularIncomesService = regularIncomesService;
		_regularExpensesService = regularExpensesService;
	}

	public async Task<ICollection<IncomeExpenseDto>> GetIncomeExpenseData(IncomeExpenseQuery query)
	{
        await _regularIncomesService.HandleAddingIncomes();
        await _regularExpensesService.HandleAddingExpenses();

        var incomeDtos = await _incomesService.GetAllForUser(query);
		var expenseDtos = await _expensesService.GetAllForUser(query);

        var incomes = _mapper.Map<ICollection<IncomeExpenseDto>>(incomeDtos);
		var expenses = _mapper.Map<ICollection<IncomeExpenseDto>>(expenseDtos);

		var incomesExpenses = incomes.Union(expenses).OrderByDescending(ie => ie.Date).ToList();
		return incomesExpenses;
    }
}
