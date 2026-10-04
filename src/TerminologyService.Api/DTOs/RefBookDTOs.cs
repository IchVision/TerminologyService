using System.ComponentModel.DataAnnotations;
using TerminologyService.Domain;

namespace TerminologyService.Api.DTOs;

public record CreateRefBookRequest
{
	[Required]
	[MaxLength(RefBook.MaxLengthCode)]
	public string Code { get; init; } = string.Empty;

	[Required]
	[MaxLength(RefBook.MaxLengthName)]
	public string Name { get; init; } = string.Empty;

	public string? Description { get; init; }
}

public record UpdateRefBookRequest
{
	[Required]
	[MaxLength(RefBook.MaxLengthCode)]
	public string Code { get; init; } = string.Empty;

	[Required]
	[MaxLength(RefBook.MaxLengthName)]
	public string Name { get; init; } = string.Empty;

	public string? Description { get; init; }
}

public record RefBookResponse(Guid Id, string Code, string Name, string? Description)
{
	public static RefBookResponse FromRefBook(RefBook value) => new(value.Id, value.Code, value.Name, value.Description);
}

public record RefBooksResponse(IEnumerable<RefBookResponse> RefBooks);