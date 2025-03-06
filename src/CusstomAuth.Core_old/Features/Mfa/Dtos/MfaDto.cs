namespace CusstomAuth.Core.Features.Mfa.Dtos;

public class MfaDto
{
    public string ManualEntryKey { get; set; }
    public string QrCodeImage { get; set; }
    public string[] RestoreCodes { get; set; }
}
