using CusstomAuth.Core.Services.Sms.Dtos;

namespace CusstomAuth.Core.Services.Sms;

public interface ISmsService
{
    Task<bool> SendSmsAsync(SmsRequestDto smsRequestDto);
}
