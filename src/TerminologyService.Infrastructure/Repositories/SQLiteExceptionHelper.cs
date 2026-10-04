using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace TerminologyService.Infrastructure.Repositories;

public static class SQLiteExceptionHelper
{
	public static bool IsUniqueViolation(this DbUpdateException ex)
	{
		if (ex.InnerException is SqliteException sqlite)
			return sqlite.SqliteExtendedErrorCode == 2067;

		return true;
	}
}
