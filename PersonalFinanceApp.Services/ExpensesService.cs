using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PersonalFinanceApp.Data.Entities;
using PersonalFinanceApp.Data.Interfaces;
using PersonalFinanceApp.Services.Exceptions;
using PersonalFinanceApp.Services.Interfaces;
using PersonalFinanceApp.Services.Models;
using PersonalFinanceApp.Services.Models.Queries;

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

    public async Task<ICollection<ExpenseDto>> GetAllForUser(IncomeExpenseQuery query)
    {
        var expenses = await GetUserExpenses(query);
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

    public async Task Update(AddExpenseDto dto, int id)
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

    private async Task<ICollection<Expense>> GetUserExpenses(IncomeExpenseQuery query)
    {
        var userId = _userContextService.TryGetUserId();
        return await _unitOfWork.Expenses.Get()
            .Include(i => i.Category)
            .Where(i => i.Category.UserId == userId)
            .Where(i => query.ExpenseCategoryId == null || i.CategoryId == query.ExpenseCategoryId)
            .Where(i => query.DateFrom == null || i.Date >= query.DateFrom)
            .Where(i => query.DateTo == null || i.Date <= query.DateTo)
            .Where(i => query.Search == null || i.Comment != null && i.Comment.ToLower().Contains(query.Search.ToLower()))
            .OrderByDescending(i => i.Date)
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
