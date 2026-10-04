namespace TerminologyService.Domain;

public class RefBook(string code, string name, string? description)
{
	public const int MaxLengthCode = 100;
	public const int MaxLengthName = 300;

	public Guid Id { get; init; } = Guid.CreateVersion7();
	public string Code { get; set; } = code;
	public string Name { get; set; } = name;
	public string? Description { get; set; } = description;

	public RefBook(Guid id, string code, string name, string? description)
		: this(code, name, description)
	{
		Id = id;
	}
}
