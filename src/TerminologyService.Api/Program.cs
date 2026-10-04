using Microsoft.EntityFrameworkCore;
using TerminologyService.Api.Exceptions;
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
		builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
		builder.Services.AddProblemDetails();

		builder.Services.AddDbContext<DataContext>(options => options.UseSqlite($"Data Source={fullPath}"));
		builder.Services.AddScoped<IRefBookRepository, RefBookRepository>();
		builder.Services.AddScoped<IVersionRefBookRepository, VersionRefBookRepository>();
		builder.Services.AddScoped<IElementRepository, ElementRepository>();
		builder.Services.AddScoped<RefBookService>();
		builder.Services.AddScoped<VersionRefBookService>();
		builder.Services.AddScoped<ElementService>();


		var app = builder.Build();

		app.UseExceptionHandler();

		app.UseHttpsRedirection();

		app.UseAuthorization();

		app.MapOpenApi();
		app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json", "TerminologyService API"));
		
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