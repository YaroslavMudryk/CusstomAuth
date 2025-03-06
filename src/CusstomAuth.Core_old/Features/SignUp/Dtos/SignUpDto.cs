using System.ComponentModel.DataAnnotations;

namespace CusstomAuth.Core.Features.SignUp.Dtos;

public class SignUpDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
    public string PasswordHint { get; set; }
}
