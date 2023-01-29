using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PersonalFinanceApp.Data.Entities;
using PersonalFinanceApp.Data.Interfaces;
using PersonalFinanceApp.Services.Exceptions;
using PersonalFinanceApp.Services.Interfaces;
using PersonalFinanceApp.Services.Models;

namespace PersonalFinanceApp.Services;

public class IncomeCategoriesService : IIncomeCategoriesService
{
	private readonly IMapper _mapper;
	private readonly IFinanceUnitOfWork _unitOfWork;
	private readonly IUserContextService _userContextService;

	public IncomeCategoriesService(
		IMapper mapper,
		IFinanceUnitOfWork unitOfWork,
		IUserContextService userContextService)
	{
		_mapper = mapper;
		_unitOfWork = unitOfWork;
		_userContextService = userContextService;
	}

	public async Task<ICollection<IncomeCategoryDto>> GetAllForUser()
	{
		var incomeCategories = await GetUserCategories();
        var incomeCategoriesDtos = _mapper.Map<ICollection<IncomeCategoryDto>>(incomeCategories);
		return incomeCategoriesDtos;
	}

	public async Task Add(IncomeCategoryDto dto)
	{
		var incomeCategory = new IncomeCategory
		{
			UserId = _userContextService.TryGetUserId(),
			Name = dto.Name
		};
		await _unitOfWork.IncomeCategories.AddAsync(incomeCategory);
		await _unitOfWork.CommitAsync();
	}

	public async Task Update(IncomeCategoryDto dto, int id)
	{
		var incomeCategory = await GetUserIncomeCategoryById(id);

		incomeCategory.Name = dto.Name;

		_unitOfWork.IncomeCategories.Update(incomeCategory);
		await _unitOfWork.CommitAsync();
	}

	public async Task Remove(int id)
	{
		var incomeCategory = await GetUserIncomeCategoryById(id);
		_unitOfWork.IncomeCategories.Remove(incomeCategory);
		await _unitOfWork.CommitAsync();
	}

	public async Task AddDefaultForUser(User user)
	{
		string[] categories = { "Other", "Salary" };

		foreach (var category in categories)
		{
			await _unitOfWork.IncomeCategories.AddAsync(new IncomeCategory
			{
				User = user,
				Name = category
            });
		}
	}

	private async Task<ICollection<IncomeCategory>> GetUserCategories()
	{
        var userId = _userContextService.TryGetUserId();
		return await _unitOfWork.IncomeCategories.Get()
            .Where(ic => ic.UserId == userId)
            .ToListAsync();
    }

    private async Task<IncomeCategory> GetUserIncomeCategoryById(int id)
    {
        var incomeCategory = await _unitOfWork.IncomeCategories.GetByIdAsync(id);
        var userId = _userContextService.TryGetUserId();
        if (incomeCategory == null || incomeCategory.UserId != userId)
            throw new NotFoundException("Category not found");
        return incomeCategory;
    }
}
