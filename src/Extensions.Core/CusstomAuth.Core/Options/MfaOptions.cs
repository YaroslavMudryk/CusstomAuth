namespace CusstomAuth.Core.Options;

public class MfaOptions
{
    public string IssuerName { get; set; } = default!;
    public bool GenerateRestoreCodes { get; set; } = true;
}
