using System.ComponentModel.DataAnnotations;
using TerminologyService.Domain;

namespace TerminologyService.Api.DTOs;

public record CreateElementRequest
{
	[Required]
	public Guid VersionRefBookId { get; init; }

	[Required]
	[MaxLength(Element.MaxLengthCode)]
	public string Code { get; init; } = string.Empty;

	[Required]
	[MaxLength(Element.MaxLengthValue)]
	public string Value { get; init; } = string.Empty;
}

public record UpdateElementRequest
{
	[Required]
	[MaxLength(Element.MaxLengthCode)]
	public string Code { get; init; } = string.Empty;

	[Required]
	[MaxLength(Element.MaxLengthValue)]
	public string Value { get; init; } = string.Empty;
}

public record ElementResponse(Guid Id, string Code, string Value)
{
	public static ElementResponse FromElement(Element value) => new(value.Id, value.Code, value.Value);
}

public record ElementsResponse(IEnumerable<ElementResponse> Elements);