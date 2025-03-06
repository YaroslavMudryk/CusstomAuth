using CusstomAuth.Core.Services.Email.Dtos;
namespace CusstomAuth.Core.Services.Email;

public class FakeEmailService : IEmailService
{
    public async Task SendEmailAsync(EmailRequestDto emailRequest)
    {
        await Task.FromResult(true);
    }
}
