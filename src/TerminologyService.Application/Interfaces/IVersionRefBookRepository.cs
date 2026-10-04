using TerminologyService.Domain;

using CSharpFunctionalExtensions;

namespace TerminologyService.Application.Interfaces;

public interface IVersionRefBookRepository
{
	public Task<Result<VersionRefBook, Errors>> SaveVersionRefBookAsync(VersionRefBook value);

	public Task<IEnumerable<VersionRefBook>> GetVersionRefBookByRefBookIdAsync(Guid refBookId);

	public Task<UnitResult<Errors>> UpdateVersionRefBookByIdAsync(Guid id, string version, DateOnly? date);

	public Task<UnitResult<Errors>> RemoveVersionRefBookByIdAsync(Guid id);
}
