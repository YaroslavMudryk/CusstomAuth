using CusstomAuth.Core.Services.Sms.Dtos;

namespace CusstomAuth.Core.Services.Sms;

public class FakeSmsService : ISmsService
{
    public async Task<bool> SendSmsAsync(SmsRequestDto smsRequestDto)
    {
        return await Task.FromResult(true);
    }
}
