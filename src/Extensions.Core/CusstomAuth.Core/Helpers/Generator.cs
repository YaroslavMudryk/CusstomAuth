using CusstomAuth.Core.Options;
using System.Security.Cryptography;
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

    public static string GetPassword(PasswordOptions options)
    {
        string Lowercase = "abcdefghijklmnopqrstuvwxyz";
        string Uppercase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        string Digits = "0123456789";
        string NonAlphanumeric = "!@#$%^&*()-_=+[]{}|;:,.<>?/";


        var requiredChars = new List<char>();
        var allChars = new StringBuilder();

        if (options.RequireLowercase)
        {
            requiredChars.Add(GetRandomChar(Lowercase));
            allChars.Append(Lowercase);
        }

        if (options.RequireUppercase)
        {
            requiredChars.Add(GetRandomChar(Uppercase));
            allChars.Append(Uppercase);
        }

        if (options.RequireDigit)
        {
            requiredChars.Add(GetRandomChar(Digits));
            allChars.Append(Digits);
        }

        if (options.RequireNonAlphanumeric)
        {
            requiredChars.Add(GetRandomChar(NonAlphanumeric));
            allChars.Append(NonAlphanumeric);
        }

        // Якщо вимог більше, ніж довжина — підлаштовуємось
        var totalLength = Math.Max(options.RequiredLength, requiredChars.Count);

        // Дозаповнюємо пароль
        while (requiredChars.Count < totalLength)
        {
            requiredChars.Add(GetRandomChar(allChars.ToString()));
        }

        // Перемішати символи
        var password = Shuffle(requiredChars);

        // Перевірити унікальність символів
        if (password.Distinct().Count() < options.RequiredUniqueChars)
        {
            return GetPassword(options); // пробуємо знову
        }

        return new string(password.ToArray());
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

    private static char GetRandomChar(string chars)
    {
        var index = RandomNumberGenerator.GetInt32(chars.Length);
        return chars[index];
    }

    private static List<char> Shuffle(List<char> list)
    {
        var rng = RandomNumberGenerator.Create();
        var shuffled = list.ToList();
        for (int i = shuffled.Count - 1; i > 0; i--)
        {
            int j = RandomNumberGenerator.GetInt32(i + 1);
            (shuffled[i], shuffled[j]) = (shuffled[j], shuffled[i]);
        }
        return shuffled;
    }
}
