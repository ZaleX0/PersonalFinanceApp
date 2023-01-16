using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalFinanceApp.Services.Interfaces;
using PersonalFinanceApp.Services.Models;

namespace PersonalFinanceApp.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class IncomesController : ControllerBase
{
    private readonly IIncomesService _incomesService;
	private readonly IIncomeCategoriesService _incomeCategoriesService;

    public IncomesController(
        IIncomesService incomesService,
        IIncomeCategoriesService incomeCategoriesService)
	{
        _incomesService = incomesService;
		_incomeCategoriesService = incomeCategoriesService;
    }

    #region Income endpoints

    [HttpGet]
    public async Task<IActionResult> GetUserIncomes()
    {
        var incomes = await _incomesService.GetAllForUser();
        return Ok(incomes);
    }

    [HttpPost]
    public async Task<IActionResult> AddIncome(AddIncomeDto dto)
    {
        await _incomesService.Add(dto);
        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateIncome(UpdateIncomeDto dto, int id)
    {
        await _incomesService.Update(dto, id);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> RemoveIncome(int id)
    {
        await _incomesService.Remove(id);
        return Ok();
    }

    #endregion

    #region Regular income endpoints

    const string REGULAR = "regular";

    // TODO: Regular income endpoints

    #endregion

    #region Category endpoints

    const string CATEGORIES = "categories";

    [HttpGet(CATEGORIES)]
	public async Task<IActionResult> GetUserCategories()
	{
		var categories = await _incomeCategoriesService.GetAllForUser();
		return Ok(categories);
	}

    [HttpPost(CATEGORIES)]
    public async Task<IActionResult> AddCategory(IncomeCategoryDto dto)
    {
        await _incomeCategoriesService.Add(dto);
        return Ok();
    }

    [HttpPut(CATEGORIES + "/{id}")]
    public async Task<IActionResult> UpdateCategory(IncomeCategoryDto dto, int id)
    {
        await _incomeCategoriesService.Update(dto, id);
        return Ok();
    }

    [HttpDelete(CATEGORIES + "/{id}")]
    public async Task<IActionResult> RemoveCategory(int id)
    {
        await _incomeCategoriesService.Remove(id);
        return Ok();
    }
    #endregion
}
