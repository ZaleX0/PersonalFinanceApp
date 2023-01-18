using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalFinanceApp.Services.Interfaces;
using PersonalFinanceApp.Services.Models;

namespace PersonalFinanceApp.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ExpensesController : ControllerBase
{
    private readonly IExpensesService _expensesService;
    private readonly IExpenseCategoriesService _expensesCategoriesService;
    private readonly IRegularExpensesService _regularExpensesService;

    public ExpensesController(
        IExpensesService expensesService,
        IExpenseCategoriesService expensesCategoriesService,
        IRegularExpensesService regularExpensesService)
    {
        _expensesService = expensesService;
        _expensesCategoriesService = expensesCategoriesService;
        _regularExpensesService = regularExpensesService;
    }

    #region Expense endpoints

    [HttpGet]
    public async Task<IActionResult> GetUserExpenses()
    {
        await _regularExpensesService.HandleAddingExpenses();
        var expenses = await _expensesService.GetAllForUser();
        return Ok(expenses);
    }

    [HttpPost]
    public async Task<IActionResult> AddExpense(AddExpenseDto dto)
    {
        await _expensesService.Add(dto);
        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateExpense(AddExpenseDto dto, int id)
    {
        await _expensesService.Update(dto, id);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> RemoveExpense(int id)
    {
        await _expensesService.Remove(id);
        return Ok();
    }

    #endregion

    #region Regular expense endpoints

    const string REGULAR = "regular";

    [HttpGet(REGULAR)]
    public async Task<IActionResult> GetUserRegularExpenses()
    {
        var regulars = await _regularExpensesService.GetAllForUser();
        return Ok(regulars);
    }

    [HttpPost(REGULAR)]
    public async Task<IActionResult> AddRegularExpense(AddRegularExpenseDto dto)
    {
        await _regularExpensesService.Add(dto);
        return Ok();
    }

    [HttpPut(REGULAR + "/{id}")]
    public async Task<IActionResult> UpdateRegularExpense(AddRegularExpenseDto dto, int id)
    {
        await _regularExpensesService.Update(dto, id);
        return Ok();
    }

    [HttpDelete(REGULAR + "/{id}")]
    public async Task<IActionResult> RemoveRegularExpense(int id)
    {
        await _regularExpensesService.Remove(id);
        return Ok();
    }

    #endregion

    #region Expense categories endpoints

    const string CATEGORIES = "categories";

    [HttpGet(CATEGORIES)]
    public async Task<IActionResult> GetUserCategories()
    {
        var categories = await _expensesCategoriesService.GetForUser();
        return Ok(categories);
    }

    [HttpPost(CATEGORIES)]
    public async Task<IActionResult> AddCategory(ExpenseCategoryDto dto)
    {
        await _expensesCategoriesService.Add(dto);
        return Ok();
    }

    [HttpPut(CATEGORIES + "/{id}")]
    public async Task<IActionResult> UpdateCategory(ExpenseCategoryDto dto, int id)
    {
        await _expensesCategoriesService.Update(dto, id);
        return Ok();
    }

    [HttpDelete(CATEGORIES + "/{id}")]
    public async Task<IActionResult> RemoveCategory(int id)
    {
        await _expensesCategoriesService.Remove(id);
        return Ok();
    }

    #endregion
}
