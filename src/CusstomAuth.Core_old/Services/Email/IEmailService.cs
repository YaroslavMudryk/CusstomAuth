using CusstomAuth.Core.Services.Email.Dtos;
namespace CusstomAuth.Core.Services.Email;

public interface IEmailService
{
    Task SendEmailAsync(EmailRequestDto emailRequestDto);
}
