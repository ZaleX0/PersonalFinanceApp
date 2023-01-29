using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalFinanceApp.Services.Interfaces;
using PersonalFinanceApp.Services.Models.Queries;

namespace PersonalFinanceApp.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class IncomeExpenseController : ControllerBase
{
	private readonly IIncomeExpenseService _incomeExpenseService;

	public IncomeExpenseController(IIncomeExpenseService incomeExpenseService)
	{
        _incomeExpenseService = incomeExpenseService;
	}

	[HttpGet]
	public async Task<IActionResult> GetIncomeExpenseData([FromQuery] IncomeExpenseQuery query)
	{
		var result = await _incomeExpenseService.GetIncomeExpenseData(query);
		return Ok(result);
	}
}
