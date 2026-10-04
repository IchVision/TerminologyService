using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;
using TerminologyService.Application;

namespace TerminologyService.Api.Helpers;

public static class ResultExtensions
{
	public static ActionResult<T> ToActionResult<T>(
		this Result<T, Errors> result,
		ControllerBase controller)
	{
		if (result.IsSuccess)
			return controller.Ok(result.Value);

		return result.Error switch
		{
			Errors.DuplicateError => controller.Conflict(),
			Errors.NotFound => controller.NotFound(),
			_ => controller.StatusCode(500),
		};
	}

	public static ActionResult ToActionResult(
		this UnitResult<Errors> result,
		ControllerBase controller)
	{
		if (result.IsSuccess)
			return controller.Ok();

		return result.Error switch
		{
			Errors.DuplicateError => controller.Conflict(),
			Errors.NotFound => controller.NotFound(),
			_ => controller.StatusCode(500),
		};
	}
}
