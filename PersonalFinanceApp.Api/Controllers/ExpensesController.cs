using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalFinanceApp.Services;
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

    public ExpensesController(
        IExpensesService expensesService,
        IExpenseCategoriesService expensesCategoriesService)
    {
        _expensesService = expensesService;
        _expensesCategoriesService = expensesCategoriesService;
    }

    #region Income endpoints

    [HttpGet]
    public async Task<IActionResult> GetUserExpenses()
    {
        var incomes = await _expensesService.GetAllForUser();
        return Ok(incomes);
    }

    [HttpPost]
    public async Task<IActionResult> AddExpense(AddExpenseDto dto)
    {
        await _expensesService.Add(dto);
        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateExpense(UpdateExpenseDto dto, int id)
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

    #region Regular income endpoints

    const string REGULAR = "regular";

    // TODO: Regular income endpoints

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
