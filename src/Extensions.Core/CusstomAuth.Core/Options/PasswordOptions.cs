namespace CusstomAuth.Core.Options;

public class PasswordOptions
{
    public int RequiredLength { get; set; } = 6;
    public int RequiredUniqueChars { get; set; } = 1;
    public bool RequireNonAlphanumeric { get; set; } = true;
    public bool RequireLowercase { get; set; } = true;
    public bool RequireUppercase { get; set; } = true;
    public bool RequireDigit { get; set; } = true;
    public string Regex { get; set; } = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$";
    public Dictionary<string, string> ErrorRegexMessages { get; set; } = new Dictionary<string, string>
        {
            { "en", "There must be a minimum of 8 characters, one uppercase letter, one lowercase letter, and one number" },
            { "uk", "Має бути мінімум 8 символів, одна велика літера, одна мала літера та 1 цифра"  }
        };
}
