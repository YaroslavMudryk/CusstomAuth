namespace CusstomAuth.Core.Options;

public class CodeOptions
{
    public int Size { get; set; } = 50;
    public bool IncludeNumbers { get; set; } = true;
    public bool IncludeLetters { get; set; } = true;
    public bool OnlyUppercase { get; set; } = false;
    public bool OnlyLowercase { get; set; } = true;
}
