namespace CusstomAuth.Core.Data;

public class MfaResponse
{
    public string ManualEntryKey { get; set; } = default!;
    public string QrCodeImage { get; set; } = default!;
    public string[] RestoreCodes { get; set; } = default!;
}
