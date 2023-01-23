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
	private readonly IIncomeExpenseService _service;

	public IncomeExpenseController(IIncomeExpenseService service)
	{
		_service = service;
	}

	[HttpGet]
	public async Task<IActionResult> GetIncomeExpenseData([FromQuery] IncomeExpenseQuery query)
	{
		var result = await _service.GetIncomeExpenseData(query);
		return Ok(result);
	}
}
