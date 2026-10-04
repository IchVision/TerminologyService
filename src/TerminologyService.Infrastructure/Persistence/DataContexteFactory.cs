using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using TerminologyService.Infrastructure.Data;

namespace TerminologyService.Infrastructure.Persistence;

public class DataContexteFactory : IDesignTimeDbContextFactory<DataContext>
{
	public DataContext CreateDbContext(string[] args)
	{
		var optionsBuilder = new DbContextOptionsBuilder<DataContext>();

		optionsBuilder.UseSqlite("Data Source=app.db");

		return new DataContext(optionsBuilder.Options);
	}
}
