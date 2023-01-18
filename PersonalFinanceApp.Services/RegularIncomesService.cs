using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PersonalFinanceApp.Data.Entities;
using PersonalFinanceApp.Data.Interfaces;
using PersonalFinanceApp.Services.Exceptions;
using PersonalFinanceApp.Services.Interfaces;
using PersonalFinanceApp.Services.Models;

namespace PersonalFinanceApp.Services;

public class RegularIncomesService : IRegularIncomesService
{
	private readonly IMapper _mapper;
	private readonly IFinanceUnitOfWork _unitOfWork;
	private readonly IUserContextService _userContextService;

	public RegularIncomesService(
		IMapper mapper,
		IFinanceUnitOfWork unitOfWork,
		IUserContextService userContextService)
	{
		_mapper = mapper;
		_unitOfWork = unitOfWork;
		_userContextService = userContextService;
	}

	public async Task Add(AddRegularIncomeDto dto)
	{
		await CheckIfUserCategoryExists(dto.CategoryId);
		var regular = _mapper.Map<RegularIncome>(dto);
		await _unitOfWork.RegularIncomes.AddAsync(regular);

        // Add first income
        await HandleAddingFirstIncome(regular);
        await _unitOfWork.CommitAsync();
	}

	public async Task<ICollection<RegularIncomeDto>> GetAllForUser()
	{
		var regulars = await GetRegularIncomes();
		var regularDtos = _mapper.Map<ICollection<RegularIncomeDto>>(regulars);
		return regularDtos;
	}

    public async Task Update(AddRegularIncomeDto dto, int id)
    {
		await CheckIfUserCategoryExists(dto.CategoryId);
        var regular = await GetById(id);

		regular.CategoryId = dto.CategoryId;
		regular.Price = dto.Price;
		regular.Comment = dto.Comment;
		regular.LastDateAdded = dto.LastDateAdded;
		regular.RepeatingNumberOfDays = dto.RepeatingNumberOfDays;

        _unitOfWork.RegularIncomes.Update(regular);
        await _unitOfWork.CommitAsync();
    }

    public async Task Remove(int id)
    {
		var regular = await GetById(id);
		_unitOfWork.RegularIncomes.Remove(regular);
		await _unitOfWork.CommitAsync();
    }

    public async Task HandleAddingIncomes()
	{
		var incomesToAdd = new List<Income>();
        var regulars = await GetRegularIncomes();
        foreach (var regular in regulars)
        {
            var nextDate = regular.LastDateAdded.AddDays(regular.RepeatingNumberOfDays);
            if (nextDate > DateTime.Today)
                continue;

            while (nextDate <= DateTime.Today)
            {
                regular.LastDateAdded = nextDate;
                incomesToAdd.Add(_mapper.Map<Income>(regular));
                nextDate = nextDate.AddDays(regular.RepeatingNumberOfDays);
            }
            _unitOfWork.RegularIncomes.Update(regular);
        }

        if (incomesToAdd.IsNullOrEmpty())
			return;

		await _unitOfWork.Incomes.AddRangeAsync(incomesToAdd);
		await _unitOfWork.CommitAsync();
	}

	private async Task HandleAddingFirstIncome(RegularIncome regular)
	{
        if (regular.LastDateAdded <= DateTime.Today)
        {
            var income = _mapper.Map<Income>(regular);
            await _unitOfWork.Incomes.AddAsync(income);
        }
    }

    private async Task<ICollection<RegularIncome>> GetRegularIncomes()
	{
		var userId = _userContextService.TryGetUserId();
		return await _unitOfWork.RegularIncomes.Get()
            .Include(r => r.Category)
			.Where(r => r.Category.UserId == userId)
            .ToListAsync();
	}

	private async Task<RegularIncome> GetById(int id)
	{
        var userId = _userContextService.TryGetUserId();
		var regular = await _unitOfWork.RegularIncomes.Get().FirstOrDefaultAsync(r => r.Category.UserId == userId && r.Id == id);
		if (regular == null)
			throw new NotFoundException("Regular income not found");
		return regular;
	}

	private async Task CheckIfUserCategoryExists(int categoryId)
	{
		var category = await _unitOfWork.IncomeCategories.GetByIdAsync(categoryId);
		var userId = _userContextService.TryGetUserId();
		if (category == null || category?.UserId != userId)
			throw new NotFoundException("Category not found");
	}
}
