using TerminologyService.Application.Interfaces;
using TerminologyService.Domain;

using CSharpFunctionalExtensions;

namespace TerminologyService.Application.Services
{
	public class VersionRefBookService
	{
		private readonly IVersionRefBookRepository _repository;

		public VersionRefBookService(IVersionRefBookRepository repository)
		{
			_repository = repository;
		}

		public Task<Result<VersionRefBook, Errors>> CreateVersionRefBookAsync(Guid refBookId, string version, DateOnly? date)
		{
			VersionRefBook versionRefBook = new(refBookId, version, date);

			return _repository.SaveVersionRefBookAsync(versionRefBook);
		}

		public Task<IEnumerable<VersionRefBook>> GetVersionRefBooksAsync(Guid refBookId) => _repository.GetVersionRefBookByRefBookIdAsync(refBookId);

		public Task<UnitResult<Errors>> UpdateVersionRefBookAsync(Guid id, string version, DateOnly? date)
		{
			return _repository.UpdateVersionRefBookByIdAsync(id, version, date);
		}

		public Task<UnitResult<Errors>> RemoveVersionRefBookByAsync(Guid id)
		{
			return _repository.RemoveVersionRefBookByIdAsync(id);
		}
	}
}
