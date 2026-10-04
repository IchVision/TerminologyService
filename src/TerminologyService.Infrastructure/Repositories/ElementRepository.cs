using TerminologyService.Application;
using TerminologyService.Application.Interfaces;
using TerminologyService.Domain;
using TerminologyService.Infrastructure.Data;

using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;

namespace TerminologyService.Infrastructure.Repositories;

public class ElementRepository(DataContext context) : IElementRepository
{
	private readonly DataContext _db = context;

	public async Task<IEnumerable<Element>> GetElementsAsync(Guid refBookId, string? inputVersion)
	{
		var result = _db.Elements.FromSql($"""
			SELECT e.*
			FROM "Elements" e
			WHERE e."VersionRefBookId" = (
				SELECT v."Id"
				FROM "VersionRefBooks" v
				WHERE v."RefBookId" = {refBookId}
					AND (
						{inputVersion} IS NOT NULL
						AND v."Version" = {inputVersion}
						OR
						{inputVersion} IS NULL
						AND v."Date" < CURRENT_DATE
					)
				ORDER BY
					v."Date" DESC NULLS LAST
				LIMIT 1
			)
			""");

		return await result.ToListAsync();
	}

	public async Task<UnitResult<Errors>> RemoveElementByIdAsync(Guid id)
	{
		var affected = await _db.Elements.Where(x => x.Id == id).ExecuteDeleteAsync();

		if (affected == 0)
			return Errors.NotFound;

		return UnitResult.Success<Errors>();
	}

	public async Task<Result<Element, Errors>> SaveElementAsync(Element element)
	{
		_db.Elements.Add(element);

		try
		{
			await _db.SaveChangesAsync();
		}
		catch (DbUpdateException ex) when (ex.IsUniqueViolation())
		{
			return Errors.DuplicateError;
		}
		
		return element;
	}

	public async Task<UnitResult<Errors>> UpdateElementByIdAsync(Guid id, string code, string value)
	{
		var affected = 0;

		try
		{
			affected = await _db.Elements
				.Where(x => x.Id == id)
				.ExecuteUpdateAsync(x => x
					.SetProperty(x => x.Code, code)
					.SetProperty(x => x.Value, value));
		}
		catch (DbUpdateException ex) when (ex.IsUniqueViolation())
		{
			return Errors.DuplicateError;
		}

		return UnitResult.Success<Errors>();
	}
}
