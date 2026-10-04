using TerminologyService.Application.Interfaces;
using TerminologyService.Domain;

using CSharpFunctionalExtensions;

namespace TerminologyService.Application.Services;

public class ElementService
{
	private readonly IElementRepository _repository;

	public ElementService(IElementRepository repository)
	{
		_repository = repository;
	}

	public Task<Result<Element, Errors>> CreateElementAsync(Guid versionRefBookId, string code, string value)
	{
		Element element = new(versionRefBookId, code, value);

		return _repository.SaveElementAsync(element);
	}

	public Task<UnitResult<Errors>> UpdateElementAsync(Guid id, string code, string value)
	{
		return _repository.UpdateElementByIdAsync(id, code, value);
	}

	public Task<UnitResult<Errors>> RemoveElementAsync(Guid id)
	{
		return _repository.RemoveElementByIdAsync(id);
	}

	public Task<IEnumerable<Element>> GetElementsAsync(Guid refBookId, string? version)
	{
		return _repository.GetElementsAsync(refBookId, version);
	}
}
