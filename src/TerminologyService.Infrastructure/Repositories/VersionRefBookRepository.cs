using TerminologyService.Application;
using TerminologyService.Application.Interfaces;
using TerminologyService.Domain;
using TerminologyService.Infrastructure.Data;

using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;

namespace TerminologyService.Infrastructure.Repositories;

public class VersionRefBookRepository(DataContext context) : IVersionRefBookRepository
{
	private readonly DataContext _db = context;

	public async Task<IEnumerable<VersionRefBook>> GetVersionRefBookByRefBookIdAsync(Guid refBookId)
	{
		return await _db.VersionRefBooks.Where(x => x.RefBookId == refBookId).ToListAsync();
	}

	public async Task<UnitResult<Errors>> RemoveVersionRefBookByIdAsync(Guid id)
	{
		var affected = await _db.VersionRefBooks.Where(x => x.Id == id).ExecuteDeleteAsync();

		if (affected == 0)
			return Errors.NotFound;

		return UnitResult.Success<Errors>();
	}

	public async Task<Result<VersionRefBook, Errors>> SaveVersionRefBookAsync(VersionRefBook value)
	{
		_db.VersionRefBooks.Add(value);

		try
		{
			await _db.SaveChangesAsync();
		}
		catch (DbUpdateException ex) when (ex.IsUniqueViolation())
		{
			return Errors.DuplicateError;
		}

		return value;
	}

	public async Task<UnitResult<Errors>> UpdateVersionRefBookByIdAsync(Guid id, string version, DateOnly? date)
	{
		int affected = 0;
		
		try
		{
			affected = await _db.VersionRefBooks
			.Where(x => x.Id == id)
			.ExecuteUpdateAsync(x => x
				.SetProperty(x => x.Version, version)
				.SetProperty(x => x.Date, date));
		}
		catch (DbUpdateException ex) when (ex.IsUniqueViolation())
		{
			return Errors.DuplicateError;
		}

		if (affected == 0)
			return Errors.NotFound;

		return UnitResult.Success<Errors>();
	}
}
