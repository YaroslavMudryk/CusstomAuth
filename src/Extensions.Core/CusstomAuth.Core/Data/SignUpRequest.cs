namespace CusstomAuth.Core.Data;

public class SignUpRequest
{
    public string Login { get; set; } = default!;
    public string Password { get; set; } = default!;
    public string PasswordHint { get; set; } = default!;

    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
}
