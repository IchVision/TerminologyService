namespace TerminologyService.Domain;

public class VersionRefBook(Guid refBookId, string version, DateOnly? date)
{
	public const int MaxLengthVersion = 50;

	public Guid Id { get; init; } = Guid.CreateVersion7();
	public Guid RefBookId { get; init; } = refBookId;
	public string Version { get; set; } = version;
	public DateOnly? Date { get; set; } = date;
}
