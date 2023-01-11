using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PersonalFinanceApp.Data.Entities;
using PersonalFinanceApp.Data.Interfaces;
using PersonalFinanceApp.Services.Models;

namespace PersonalFinanceApp.Services;

public class IncomeCategoriesService
{
	private readonly IMapper _mapper;
	private readonly IFinanceUnitOfWork _unitOfWork;

	public IncomeCategoriesService(IMapper mapper, IFinanceUnitOfWork unitOfWork)
	{
		_mapper = mapper;
		_unitOfWork = unitOfWork;
	}

	public async Task<ICollection<IncomeCategoryDto>> GetAllAsync()
	{
		var incomeCategories = await _unitOfWork.IncomeCategories.Get().ToListAsync();


	}
}
