namespace TerminologyService.Domain;

public class Element(Guid versionRefBookId, string code, string value)
{
	public const int MaxLengthCode = 100;
	public const int MaxLengthValue = 300;

	public Guid Id { get; init; } = Guid.CreateVersion7();
	public Guid VersionRefBookId { get; set; } = versionRefBookId;
	public string Code { get; set; } = code;
	public string Value { get; set; } = value;
}
