using TerminologyService.Application;
using TerminologyService.Application.Interfaces;
using TerminologyService.Domain;
using TerminologyService.Infrastructure.Data;

using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;

namespace TerminologyService.Infrastructure.Repositories;

public class RefBookRepository(DataContext dataContext) : IRefBookRepository
{
	private readonly DataContext _db = dataContext;

	public async Task<IEnumerable<RefBook>> GetListRefBooksAsync(DateOnly? date)
	{
		IQueryable<RefBook> result = _db.RefBooks;

		if (date is not null)
			result = result.Where(refBook =>
			_db.VersionRefBooks.Any(version =>
				version.RefBookId == refBook.Id &&
				version.Date <= date));

		return await result.ToListAsync();
	}

	public async Task<UnitResult<Errors>> RemoveRefBookByIdAsync(Guid id)
	{
		var affected = await _db.RefBooks.Where(x => x.Id == id).ExecuteDeleteAsync();

		if (affected == 0)
			return Errors.NotFound;

		return UnitResult.Success<Errors>();
	}

	public async Task<Result<RefBook, Errors>> SaveRefBookAsync(RefBook value)
	{
		_db.RefBooks.Add(value);

		try
		{
			await _db.SaveChangesAsync();
		}
		catch (DbUpdateException ex) when (ex.IsUniqueViolation())
		{
			return Result.Failure<RefBook, Errors>(Errors.DuplicateError);
		}
		catch (Exception)
		{
			return Result.Failure<RefBook, Errors>(Errors.UnexpectedError);
		}

		return value;
	}

	public async Task<UnitResult<Errors>> UpdateRefBookAsync(RefBook value)
	{
		var affected = await _db.RefBooks
			.Where(x => x.Id == value.Id)
			.ExecuteUpdateAsync(x => x
				.SetProperty(x => x.Code, value.Code)
				.SetProperty(x => x.Name, value.Name)
				.SetProperty(x => x.Description, value.Description));

		if (affected == 0)
			return Errors.NotFound;

		return UnitResult.Success<Errors>();
	}
}
