using TerminologyService.Application.Interfaces;
using TerminologyService.Domain;

using CSharpFunctionalExtensions;

namespace TerminologyService.Application.Services;

public class RefBookService(IRefBookRepository repository)
{
	private readonly IRefBookRepository _repository = repository;

	public Task<Result<RefBook, Errors>> CreateRefBookAsync(string code, string name, string? description)
	{
		RefBook refBook = new(code, name, description);

		return _repository.SaveRefBookAsync(refBook);
	}

	public Task<IEnumerable<RefBook>> GetRefBooksAsync(DateOnly? date) => _repository.GetListRefBooksAsync(date);

	public async Task<Result<RefBook, Errors>> UpdateRefBookAsync(Guid id, string code, string name, string? description)
	{
		RefBook refBook = new(id, code, name, description);

		var result = await _repository.UpdateRefBookAsync(refBook);

		return result.IsSuccess ? refBook : result.Error;	
	}

	public Task<UnitResult<Errors>> RemoveRefBookAsync(Guid id)
	{
		return _repository.RemoveRefBookByIdAsync(id);
	}
}