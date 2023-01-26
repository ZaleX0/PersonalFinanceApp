using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PersonalFinanceApp.Data.Entities;
using PersonalFinanceApp.Data.Interfaces;
using PersonalFinanceApp.Services.Exceptions;
using PersonalFinanceApp.Services.Interfaces;
using PersonalFinanceApp.Services.Models;

namespace PersonalFinanceApp.Services;

public class ExpenseCategoriesService : IExpenseCategoriesService
{
    private readonly IMapper _mapper;
    private readonly IFinanceUnitOfWork _unitOfWork;
    private readonly IUserContextService _userContextService;

    public ExpenseCategoriesService(
        IMapper mapper,
        IFinanceUnitOfWork unitOfWork,
        IUserContextService userContextService)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _userContextService = userContextService;
    }

    public async Task<ICollection<ExpenseCategoryDto>> GetForUser()
    {
        var expenseCategories = await GetUserCategories();
        var expenseCategoriesDtos = _mapper.Map<ICollection<ExpenseCategoryDto>>(expenseCategories);
        return expenseCategoriesDtos;
    }

    public async Task Add(ExpenseCategoryDto dto)
    {
        var expenseCategory = new ExpenseCategory
        {
            UserId = _userContextService.TryGetUserId(),
            Name = dto.Name
        };
        await _unitOfWork.ExpenseCategories.AddAsync(expenseCategory);
        await _unitOfWork.CommitAsync();
    }

    public async Task Update(ExpenseCategoryDto dto, int id)
    {
        var expenseCategory = await GetUserExpenseCategoryById(id);

        expenseCategory.Name = dto.Name;

        _unitOfWork.ExpenseCategories.Update(expenseCategory);
        await _unitOfWork.CommitAsync();
    }

    public async Task Remove(int id)
    {
        var expenseCategory = await GetUserExpenseCategoryById(id);
        _unitOfWork.ExpenseCategories.Remove(expenseCategory);
        await _unitOfWork.CommitAsync();
    }

    public async Task AddDefaultForUser(User user)
    {
        string[] categories = { "Other", "Housing", "Transportation", "Food" };

        foreach (var category in categories)
        {
            await _unitOfWork.ExpenseCategories.AddAsync(new ExpenseCategory
            {
                User = user,
                Name = category
            });
        }
    }

    private async Task<ICollection<ExpenseCategory>> GetUserCategories()
    {
        var userId = _userContextService.TryGetUserId();
        return await _unitOfWork.ExpenseCategories.Get()
            .Where(ec => ec.UserId == userId)
            .ToListAsync();
    }

    private async Task<ExpenseCategory> GetUserExpenseCategoryById(int id)
    {
        var userId = _userContextService.TryGetUserId();
        var expenseCategory = await _unitOfWork.ExpenseCategories.GetByIdAsync(id);
        if (expenseCategory == null || expenseCategory.UserId != userId)
            throw new NotFoundException("Category not found");
        return expenseCategory;
    }
}
