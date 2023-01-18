using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PersonalFinanceApp.Data.Entities;
using PersonalFinanceApp.Data.Interfaces;
using PersonalFinanceApp.Services.Exceptions;
using PersonalFinanceApp.Services.Interfaces;
using PersonalFinanceApp.Services.Models;
using System.Diagnostics;

namespace PersonalFinanceApp.Services;

public class IncomesService : IIncomesService
{
	private readonly IMapper _mapper;
	private readonly IFinanceUnitOfWork _unitOfWork;
	private readonly IUserContextService _userContextService;

	public IncomesService(IMapper mapper, IFinanceUnitOfWork unitOfWork, IUserContextService userContextService)
	{
		_mapper = mapper;
		_unitOfWork = unitOfWork;
		_userContextService = userContextService;
	}

	public async Task<ICollection<IncomeDto>> GetAllForUser()
	{
        var incomes = await GetUserIncomes();
		var incomeDtos = _mapper.Map<ICollection<IncomeDto>>(incomes);
		return incomeDtos;
    }

	public async Task<IncomeDto> GetByIdForUser(int id)
	{
		var income = await GetUserIncomeById(id);
        var incomeDto = _mapper.Map<IncomeDto>(income);
		return incomeDto;
    }

    public async Task Add(AddIncomeDto dto)
	{
		await CheckIfUserCategoryExists(dto.CategoryId);
		var income = _mapper.Map<Income>(dto);
		await _unitOfWork.Incomes.AddAsync(income);
		await _unitOfWork.CommitAsync();
	}

	public async Task Update(AddIncomeDto dto, int id)
	{
		await CheckIfUserCategoryExists(dto.CategoryId);
		var income = await GetUserIncomeById(id);

        income.Date = dto.Date;
		income.Price = dto.Price;
		income.Comment = dto.Comment;
		income.CategoryId = dto.CategoryId;

		_unitOfWork.Incomes.Update(income);
		await _unitOfWork.CommitAsync();
	}

	public async Task Remove(int id)
	{
		var income = await GetUserIncomeById(id);
		_unitOfWork.Incomes.Remove(income);
		await _unitOfWork.CommitAsync();
	}

	private async Task<Income> GetUserIncomeById(int id)
	{
        var userId = _userContextService.TryGetUserId();
		var income = await _unitOfWork.Incomes.Get().FirstOrDefaultAsync(i => i.Category.UserId == userId && i.Id == id);
        if (income == null)
            throw new NotFoundException("Income not found");
		return income;
    }

	private async Task<ICollection<Income>> GetUserIncomes()
	{
		var userId = _userContextService.TryGetUserId();
		return await _unitOfWork.Incomes.Get()
			.Include(i => i.Category)
			.Where(i => i.Category.UserId == userId)
			.ToListAsync();
    }

	private async Task CheckIfUserCategoryExists(int categoryId)
	{
		var category = await _unitOfWork.IncomeCategories.GetByIdAsync(categoryId);
		var userId = _userContextService.TryGetUserId();
		if (category == null || category?.Id != userId)
			throw new NotFoundException("Category not found");
	}
}
