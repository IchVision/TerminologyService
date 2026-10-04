using CSharpFunctionalExtensions;
using TerminologyService.Domain;

namespace TerminologyService.Application.Interfaces;

public interface IRefBookRepository
{
	public Task<Result<RefBook, Errors>> SaveRefBookAsync(RefBook value);
	public Task<IEnumerable<RefBook>> GetListRefBooksAsync(DateOnly? date);
	public Task<UnitResult<Errors>> UpdateRefBookAsync(RefBook value);
	public Task<UnitResult<Errors>> RemoveRefBookByIdAsync(Guid id);
}