using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PersonalFinanceApp.Data.Entities;
using PersonalFinanceApp.Data.Interfaces;
using PersonalFinanceApp.Services.Exceptions;
using PersonalFinanceApp.Services.Interfaces;
using PersonalFinanceApp.Services.Models;

namespace PersonalFinanceApp.Services;

public class RegularExpensesService : IRegularExpensesService
{
    private readonly IMapper _mapper;
    private readonly IFinanceUnitOfWork _unitOfWork;
    private readonly IUserContextService _userContextService;

    public RegularExpensesService(
        IMapper mapper,
        IFinanceUnitOfWork unitOfWork,
        IUserContextService userContextService)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _userContextService = userContextService;
    }

    public async Task Add(AddRegularExpenseDto dto)
    {
        await CheckIfUserCategoryExists(dto.CategoryId);
        var regular = _mapper.Map<RegularExpense>(dto);
        await _unitOfWork.RegularExpenses.AddAsync(regular);

        // Add first expense
        await HandleAddingFirstExpense(regular);
        await _unitOfWork.CommitAsync();
    }

    public async Task<ICollection<RegularExpenseDto>> GetAllForUser()
    {
        var regulars = await GetRegularExpenses();
        var regularDtos = _mapper.Map<ICollection<RegularExpenseDto>>(regulars);
        return regularDtos;
    }

    public async Task Update(AddRegularExpenseDto dto, int id)
    {
        await CheckIfUserCategoryExists(dto.CategoryId);
        var regular = await GetById(id);

        regular.CategoryId = dto.CategoryId;
        regular.Price = dto.Price;
        regular.Comment = dto.Comment;
        regular.LastDateAdded = dto.LastDateAdded;
        regular.RepeatingNumberOfDays = dto.RepeatingNumberOfDays;

        _unitOfWork.RegularExpenses.Update(regular);
        await _unitOfWork.CommitAsync();
    }

    public async Task Remove(int id)
    {
        var regular = await GetById(id);
        _unitOfWork.RegularExpenses.Remove(regular);
        await _unitOfWork.CommitAsync();
    }

    public async Task HandleAddingExpenses()
    {
        var expensesToAdd = new List<Expense>();
        var regulars = await GetRegularExpenses();
        foreach (var regular in regulars)
        {
            var nextDate = regular.LastDateAdded.AddDays(regular.RepeatingNumberOfDays);
            if (nextDate > DateTime.Today)
                continue;

            while (nextDate <= DateTime.Today)
            {
                regular.LastDateAdded = nextDate;
                expensesToAdd.Add(_mapper.Map<Expense>(regular));
                nextDate = nextDate.AddDays(regular.RepeatingNumberOfDays);
            }
            _unitOfWork.RegularExpenses.Update(regular);
        }

        if (expensesToAdd.IsNullOrEmpty())
            return;

        await _unitOfWork.Expenses.AddRangeAsync(expensesToAdd);
        await _unitOfWork.CommitAsync();
    }

    private async Task HandleAddingFirstExpense(RegularExpense regular)
    {
        if (regular.LastDateAdded <= DateTime.Today)
        {
            var expense = _mapper.Map<Expense>(regular);
            await _unitOfWork.Expenses.AddAsync(expense);
        }
    }

    private async Task<ICollection<RegularExpense>> GetRegularExpenses()
    {
        var userId = _userContextService.TryGetUserId();
        return await _unitOfWork.RegularExpenses.Get()
            .Include(r => r.Category)
            .Where(r => r.Category.UserId == userId)
            .ToListAsync();
    }

    private async Task<RegularExpense> GetById(int id)
    {
        var userId = _userContextService.TryGetUserId();
        var regular = await _unitOfWork.RegularExpenses.Get().FirstOrDefaultAsync(r => r.Category.UserId == userId && r.Id == id);
        if (regular == null)
            throw new NotFoundException("Regular expense not found");
        return regular;
    }

    private async Task CheckIfUserCategoryExists(int categoryId)
    {
        var category = await _unitOfWork.ExpenseCategories.GetByIdAsync(categoryId);
        var userId = _userContextService.TryGetUserId();
        if (category == null || category?.UserId != userId)
            throw new NotFoundException("Category not found");
    }
}
