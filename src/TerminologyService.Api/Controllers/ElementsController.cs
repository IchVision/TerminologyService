using TerminologyService.Api.DTOs;
using TerminologyService.Api.Helpers;
using TerminologyService.Application.Services;

using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;

namespace TerminologyService.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ElementsController : ControllerBase
{
	private readonly ElementService _service;

	public ElementsController(ElementService service) => _service = service;

	[HttpPost]
	public async Task<ActionResult<ElementResponse>> CreateElement([FromBody] CreateElementRequest request)
	{
		var result = await _service
			.CreateElementAsync(request.VersionRefBookId, request.Code, request.Value)
			.Map(ElementResponse.FromElement);

		return result.ToActionResult(this);
	}

	[HttpPut]
	public async Task<IActionResult> UpdateElement(Guid id, [FromBody] UpdateElementRequest request)
	{
		var result = await _service.UpdateElementAsync(id, request.Code, request.Value);

		return result.ToActionResult(this);
	}

	[HttpDelete]
	public async Task<IActionResult> RemoveElement(Guid id)
	{
		var result = await _service.RemoveElementAsync(id);

		return result.ToActionResult(this);
	}
}
