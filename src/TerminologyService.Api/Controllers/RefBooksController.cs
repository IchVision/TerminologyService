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
	private readonly RefBookService _serviceRefBook;
	private readonly VersionRefBookService _serviceVersion;
	private readonly ElementService _serviceElement;

	public RefBooksController(
		RefBookService refBookService,
		VersionRefBookService versionService,
		ElementService serviceElement)
	{
		_serviceRefBook = refBookService;
		_serviceVersion = versionService;
		_serviceElement = serviceElement;
	}

	[HttpPost]
	public async Task<ActionResult<RefBookResponse>> CreateRefBook([FromBody] CreateRefBookRequest request)
	{
		var result = await _serviceRefBook
			.CreateRefBookAsync(request.Code, request.Name, request.Description)
			.Map(RefBookResponse.FromRefBook);

		return result.ToActionResult(this);
	}

	[HttpGet]
	public async Task<ActionResult<RefBooksResponse>> GetListRefBooks([FromQuery] DateOnly? date)
	{
		var refBooks = await _serviceRefBook.GetRefBooksAsync(date);

		RefBooksResponse listRefBooksResponse = new(refBooks.Select(RefBookResponse.FromRefBook));

		return Ok(listRefBooksResponse);
	}

	[HttpPut]
	public async Task<ActionResult<RefBookResponse>> UpdateRefBook(Guid id, [FromBody] UpdateRefBookRequest request)
	{
		var result = await _serviceRefBook
			.UpdateRefBookAsync(id, request.Code, request.Name, request.Description)
			.Map(RefBookResponse.FromRefBook);

		return result.ToActionResult(this);
	}

	[HttpDelete]
	public async Task<IActionResult> RemoveRefBook(Guid id)
	{
		var result = await _serviceRefBook.RemoveRefBookAsync(id);

		return result.ToActionResult(this);
	}

	[HttpGet("{refBookId:guid}/versions")]
	public async Task<ActionResult<ListVersionRefBookResponse>> GetVersionsRefBook(Guid refBookId)
	{
		var versions = await _serviceVersion
			.GetVersionRefBooksAsync(refBookId);

		ListVersionRefBookResponse response = new(
			refBookId,
			versions.Select(VersionRefBookResponse.FromVersionRefBook));

		return Ok(response);
	}

	[HttpGet("{refBookId:guid}/elements")]
	public async Task<ActionResult<ElementsResponse>> GetElementsRefBook(Guid refBookId, [FromQuery] string? version)
	{
		var elements = await _serviceElement.GetElementsAsync(refBookId, version);

		ElementsResponse response = new(elements.Select(ElementResponse.FromElement));
		
		return Ok(response);
	}
}
