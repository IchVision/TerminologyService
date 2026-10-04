using Microsoft.EntityFrameworkCore;
using TerminologyService.Application.Interfaces;
using TerminologyService.Application.Services;
using TerminologyService.Infrastructure.Data;
using TerminologyService.Infrastructure.Repositories;

public partial class Program
{
	private static void Main(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);

		var dbPath = builder.Configuration["Database:Path"]
			?? throw new InvalidOperationException("Database path is not configured.");

		var fullPath = Path.GetFullPath(
			Path.Combine(builder.Environment.ContentRootPath, dbPath));

		// Add services to the container.

		builder.Services.AddControllers();
		builder.Services.AddOpenApi();
		builder.Services.AddDbContext<DataContext>(options => options.UseSqlite($"Data Source={fullPath}"));
		builder.Services.AddScoped<IRefBookRepository, RefBookRepository>();
		builder.Services.AddScoped<RefBookService>();

		var app = builder.Build();

		app.MapOpenApi();

		app.UseHttpsRedirection();

		app.UseAuthorization();

		app.MapControllers();

		// Apply migrations
		using (var scope = app.Services.CreateScope())
		{
			var db = scope.ServiceProvider.GetRequiredService<DataContext>();

			db.Database.Migrate();
		}

		app.Run();
	}
}