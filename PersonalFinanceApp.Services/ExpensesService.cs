using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PersonalFinanceApp.Data.Entities;
using PersonalFinanceApp.Data.Interfaces;
using PersonalFinanceApp.Services.Exceptions;
using PersonalFinanceApp.Services.Interfaces;
using PersonalFinanceApp.Services.Models;
using System.Diagnostics;

namespace PersonalFinanceApp.Services;

public class ExpensesService : IExpensesService
{
    private readonly IMapper _mapper;
    private readonly IFinanceUnitOfWork _unitOfWork;
    private readonly IUserContextService _userContextService;

    public ExpensesService(
        IMapper mapper,
        IFinanceUnitOfWork unitOfWork,
        IUserContextService userContextService)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _userContextService = userContextService;
    }

    public async Task<ICollection<ExpenseDto>> GetAllForUser()
    {
        var expenses = await GetUserExpenses();
        var expenseDtos = _mapper.Map<ICollection<ExpenseDto>>(expenses);
        return expenseDtos;
    }

    public async Task<ExpenseDto> GetByIdForUser(int id)
    {
        var expense = await GetUserExpenseById(id);
        var expenseDto = _mapper.Map<ExpenseDto>(expense);
        return expenseDto;
    }

    public async Task Add(AddExpenseDto dto)
    {
        await CheckIfUserCategoryExists(dto.CategoryId);
        var expense = _mapper.Map<Expense>(dto);
        await _unitOfWork.Expenses.AddAsync(expense);
        await _unitOfWork.CommitAsync();
    }

    public async Task Update(UpdateExpenseDto dto, int id)
    {
        await CheckIfUserCategoryExists(dto.CategoryId);
        var expense = await GetUserExpenseById(id);

        expense.Date = dto.Date;
        expense.Price = dto.Price;
        expense.Comment = dto.Comment;
        expense.CategoryId = dto.CategoryId;

        _unitOfWork.Expenses.Update(expense);
        await _unitOfWork.CommitAsync();
    }

    public async Task Remove(int id)
    {
        var expense = await GetUserExpenseById(id);
        _unitOfWork.Expenses.Remove(expense);
        await _unitOfWork.CommitAsync();
    }

    private async Task<Expense> GetUserExpenseById(int id)
    {
        var userId = _userContextService.TryGetUserId();
        var expense = await _unitOfWork.Expenses.Get().FirstOrDefaultAsync(e => e.Category.UserId == userId && e.Id == id);
        if (expense == null)
            throw new NotFoundException("Expense not found");
        return expense;
    }

    private async Task<ICollection<Expense>> GetUserExpenses()
    {
        var userId = _userContextService.TryGetUserId();
        return await _unitOfWork.Expenses.Get()
            .Include(e => e.Category)
            .Where(e => e.Category.UserId == userId)
            .ToListAsync();
    }

    private async Task CheckIfUserCategoryExists(int categoryId)
    {
        var category = await _unitOfWork.ExpenseCategories.GetByIdAsync(categoryId);
        var userId = _userContextService.UserId;

        if (category == null || category?.Id != userId)
            throw new NotFoundException("Category not found");
    }
}
