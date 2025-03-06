namespace CusstomAuth;

public interface ICurrentContext
{
    string GetIp();
    bool IsAdmin();
    bool IsAuthenticated();
    CurrentUser User { get; }
}
