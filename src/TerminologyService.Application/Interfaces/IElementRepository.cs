using TerminologyService.Domain;

using CSharpFunctionalExtensions;

namespace TerminologyService.Application.Interfaces;

public interface IElementRepository
{
	public Task<Result<Element, Errors>> SaveElementAsync(Element element);

	public Task<IEnumerable<Element>> GetElementsAsync(Guid refBookId, string? version);

	public Task<UnitResult<Errors>> UpdateElementByIdAsync(Guid id, string code, string value);

	public Task<UnitResult<Errors>> RemoveElementByIdAsync(Guid id);
}
