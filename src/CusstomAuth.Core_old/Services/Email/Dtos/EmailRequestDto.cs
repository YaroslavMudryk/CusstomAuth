namespace CusstomAuth.Core.Services.Email.Dtos;

public class EmailRequestDto
{
    public string UserEmail { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
}
