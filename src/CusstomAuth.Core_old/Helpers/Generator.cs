using CusstomAuth.Core.Options;
using System.Text;

namespace CusstomAuth.Core.Helpers;

public class Generator
{
    private static string _upperChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private static string _lowerChars = "abcdefghijklmnopqrstuvwxyz";
    private static string _numbersChars = "0123456789";
    private static string _chars = $"{_upperChars}{_numbersChars}{_lowerChars}";

    public static string GetConfirmCode(CodeOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        var codeBuilder = new StringBuilder();
        var random = new Random();

        string chars = "";
        if (options.IncludeNumbers)
            chars += "0123456789";
        if (options.IncludeLetters)
            chars += "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

        if (chars.Length == 0)
            throw new ArgumentException("At least one of IncludeNumbers or IncludeLetters must be true.");

        for (int i = 0; i < options.Size; i++)
        {
            codeBuilder.Append(chars[random.Next(chars.Length)]);
        }

        var code = codeBuilder.ToString();

        if (options.OnlyLowercase)
            code = code.ToLower();

        if (options.OnlyUppercase)
            code = code.ToUpper();

        return code;
    }

    public static string[] GetRestoreCodes()
    {
        var codes = new string[8];

        for (int i = 0; i < 8; i++)
        {
            codes[i] = Guid.NewGuid().ToString("N").ToUpper().Substring(0, 8);
        }

        return codes;
    }

    public static string GetUserName()
    {
        var random = new Random();
        const string chars = "abcdefghijklmnopqrstuvwxyz";
        var length = random.Next(4, 25);

        var usernameChars = new char[length];
        for (int i = 0; i < length; i++)
        {
            usernameChars[i] = chars[random.Next(chars.Length)];
        }

        return new string(usernameChars);
    }

    public static string CreateAppId()
    {
        return GetUniqCode(4);
    }

    public static string CreateAppSecret()
    {
        return GetString(70);
    }

    public static string GetUniqCode(int sections)
    {
        var commonWords = sections * 4;
        var commonCountOfSymbols = commonWords + (sections - 1);
        var stringChars = new char[commonCountOfSymbols];
        var positons = GetHyphenPositions(sections);
        var random = new Random();
        for (int i = 0; i < stringChars.Length; i++)
        {
            if (positons.Contains(i))
            {
                stringChars[i] = '-';
                continue;
            }
            stringChars[i] = _chars[random.Next(_chars.Length)];
        }
        return new String(stringChars);
    }

    private static int[] GetHyphenPositions(int sections)
    {
        var pos = new int[sections - 1];
        var baseIndex = 4;
        for (int i = 0; i < pos.Length; i++)
        {
            pos[i] = baseIndex;
            baseIndex += 4 + 1;
        }
        return pos;
    }

    public static string GetString(int length, bool IsUpper = false, bool IsLowwer = false)
    {
        var stringChars = new char[length];
        var random = new Random();
        for (int i = 0; i < stringChars.Length; i++)
        {
            stringChars[i] = _chars[random.Next(_chars.Length)];
        }
        var result = new string(stringChars);
        return IsUpper ? result.ToUpper() : IsLowwer ? result.ToLower() : result;
    }
}
