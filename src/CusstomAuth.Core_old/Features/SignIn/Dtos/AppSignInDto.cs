using System.ComponentModel.DataAnnotations;

namespace CusstomAuth.Core.Features.SignIn.Dtos;

public class AppSignInDto
{
    [Required, StringLength(30)]
    public string Id { get; set; }
    [Required, StringLength(70)]
    public string Secret { get; set; }
    [Required, StringLength(50, MinimumLength = 1)]
    public string Version { get; set; }
}
