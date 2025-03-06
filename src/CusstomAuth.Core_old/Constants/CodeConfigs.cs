using CusstomAuth.Core.Options;

namespace CusstomAuth.Core.Constants;

public static class CodeConfigs
{
    public const string ConfirmAccount = "ConfirmAccount";
    public const string ConfirmEmail= "ConfirmEmail";
    public const string ConfirmPhone = "ConfirmPhone";

    public static Dictionary<string, CodeOptions> Defaults = new()
    {
        { ConfirmAccount, new CodeOptions { Size = 64, IncludeLetters = true, IncludeNumbers = true, OnlyLowercase = true, OnlyUppercase = false } },
        { ConfirmEmail, new CodeOptions { Size = 36, IncludeLetters = true, IncludeNumbers = true, OnlyLowercase = true, OnlyUppercase = false } },
        { ConfirmPhone, new CodeOptions { Size = 36, IncludeLetters = true, IncludeNumbers = true, OnlyLowercase = true, OnlyUppercase = false } }
    };
}
