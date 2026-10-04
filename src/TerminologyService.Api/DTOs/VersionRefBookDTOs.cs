using System.Collections;
using System.ComponentModel.DataAnnotations;
using TerminologyService.Domain;

namespace TerminologyService.Api.DTOs;

public record CreateVersionRefBookRequest
{
	[Required]
	public Guid RefBookId { get; init; }

	[Required]
	[MaxLength(VersionRefBook.MaxLengthVersion)]
	public string Version { get; init; } = string.Empty;

	public DateOnly? Date { get; init; }
}

public record UpdateVersionRefBookRequest
{
	[Required]
	[MaxLength(VersionRefBook.MaxLengthVersion)]
	public string Version { get; init; } = string.Empty;

	public DateOnly? Date { get; init; }
}

public record VersionRefBookResponse(Guid Id, string Version, DateOnly? Date)
{
	public static VersionRefBookResponse FromVersionRefBook(VersionRefBook value)
	{
		return new(value.Id, value.Version, value.Date);
	}
}

public record ListVersionRefBookResponse(Guid refBookId, IEnumerable<VersionRefBookResponse> versions);

