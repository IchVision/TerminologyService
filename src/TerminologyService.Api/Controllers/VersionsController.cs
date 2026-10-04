using TerminologyService.Api.DTOs;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TerminologyService.Application.Services;
using CSharpFunctionalExtensions;
using TerminologyService.Api.Helpers;

namespace TerminologyService.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class VersionsController : ControllerBase
{
	private readonly VersionRefBookService _service;

	public VersionsController(VersionRefBookService service) => _service = service;

	[HttpPost]
	public async Task<ActionResult<VersionRefBookResponse>> CreateVersionRefBook([FromBody] CreateVersionRefBookRequest request)
	{
		var result = await _service
			.CreateVersionRefBookAsync(request.RefBookId, request.Version, request.Date)
			.Map(VersionRefBookResponse.FromVersionRefBook);

		return result.ToActionResult(this);
	}

	[HttpPut]
	public async Task<IActionResult> UpdateVersionRefBook(Guid id, [FromBody] UpdateVersionRefBookRequest request)
	{
		var result = await _service.UpdateVersionRefBookAsync(id, request.Version, request.Date);

		return result.ToActionResult(this);
	}

	[HttpDelete]
	public async Task<IActionResult> RemoveVersionRefBook(Guid id)
	{
		var result = await _service.RemoveVersionRefBookByAsync(id);

		return result.ToActionResult(this);
	}
}
