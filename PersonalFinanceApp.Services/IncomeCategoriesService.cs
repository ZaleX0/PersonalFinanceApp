using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PersonalFinanceApp.Data.Entities;
using PersonalFinanceApp.Data.Interfaces;
using PersonalFinanceApp.Services.Exceptions;
using PersonalFinanceApp.Services.Interfaces;
using PersonalFinanceApp.Services.Models;

namespace PersonalFinanceApp.Services;

public class IncomeCategoriesService
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

	public async Task<ICollection<IncomeCategoryDto>> GetByUserAsync()
	{
        int? userId = _userContextService.GetUserId;
        if (userId == null)
            throw new NotFoundException("User not found");

        var incomeCategories = await _unitOfWork.IncomeCategories.Get()
			.Where(ic => ic.UserId == userId)
			.ToListAsync();

		var incomeCategoriesDtos = _mapper.Map<ICollection<IncomeCategoryDto>>(incomeCategories);
		return incomeCategoriesDtos;
	}
}
