using TerminologyService.Application.Services;
using TerminologyService.Api.DTOs;
using TerminologyService.Api.Helpers;

using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;

namespace TerminologyService.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RefBooksController : ControllerBase
{
	private readonly RefBookService _service;

	public RefBooksController(RefBookService service) => _service = service;

	[HttpPost]
	public async Task<ActionResult<RefBookResponse>> CreateRefBook([FromBody] CreateRefBookRequest request)
	{
		var result = await _service
			.CreateRefBookAsync(request.Code, request.Name, request.Description)
			.Map(RefBookResponse.FromRefBook);

		return result.ToActionResult(this);
	}

	[HttpGet]
	public async Task<ActionResult<ListRefBooksResponse>> GetListRefBooks([FromQuery] DateOnly? date)
	{
		var refBooks = await _service.GetRefBooksAsync(date);

		ListRefBooksResponse listRefBooksResponse = new(refBooks.Select(RefBookResponse.FromRefBook));

		return Ok(listRefBooksResponse);
	}

	[HttpPut]
	public async Task<ActionResult<RefBookResponse>> UpdateRefBook(Guid id, [FromBody] UpdateRefBookRequest request)
	{
		var result = await _service
			.UpdateRefBookAsync(id, request.Code, request.Name, request.Description)
			.Map(RefBookResponse.FromRefBook);

		return result.ToActionResult(this);
	}

	[HttpDelete]
	public async Task<IActionResult> RemoveRefBook(Guid id)
	{
		var result = await _service.RemoveRefBookAsync(id);

		return result.ToActionResult(this);
	}
}
