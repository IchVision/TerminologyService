using TerminologyService.Domain;

using Microsoft.EntityFrameworkCore;

namespace TerminologyService.Infrastructure.Data;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
	public DbSet<RefBook> RefBooks => Set<RefBook>();
	public DbSet<VersionRefBook> VersionRefBooks => Set<VersionRefBook>();
	public DbSet<Element> Elements => Set<Element>();

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<RefBook>().HasIndex(x => x.Code).IsUnique();
	}
}
