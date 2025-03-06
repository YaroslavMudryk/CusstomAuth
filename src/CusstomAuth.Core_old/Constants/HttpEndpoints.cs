using CusstomAuth.Core.Options;

namespace CusstomAuth.Core.Constants;

public static class HttpEndpoints
{
    public const string SignUp = "/api/identity/signup";
    public const string SignIn = "/api/identity/signin";
    public const string Sessions = "/api/identity/sessions";
    public const string Confirm = "/api/identity/confirm";
    public const string SendConfirm = "/api/identity/send-confirm";
    public const string RefreshToken = "/api/identity/refresh";
    public const string SignOut = "/api/identity/signout";
    public const string Mfa = "/api/identity/mfa";
    public const string SignInMfa = "/api/identity/signin-mfa";
    public const string Devices = "/api/identity/devices";
    public const string Init = "/api/seed";

    public static Dictionary<string, EndpointOptions> Default = new Dictionary<string, EndpointOptions>
    {
        { HttpActions.SignUpAction, new EndpointOptions { Endpoint = SignUp, IsAvailable = true, HttpMethod = HttpMethod.Post.Method, IsSecure = false } },
        { HttpActions.SignInAction, new EndpointOptions { Endpoint = SignIn, IsAvailable = true, HttpMethod = HttpMethod.Post.Method, IsSecure = false } },
        { HttpActions.SessionsAction, new EndpointOptions { Endpoint = Sessions, IsAvailable = true, HttpMethod = HttpMethod.Get.Method, IsSecure = true } },
        { HttpActions.CloseSessionsAction, new EndpointOptions { Endpoint = Sessions, IsAvailable = true, HttpMethod = HttpMethod.Delete.Method, IsSecure = true } },
        { HttpActions.ConfirmAction, new EndpointOptions { Endpoint = Confirm, IsAvailable = true, HttpMethod = HttpMethod.Post.Method, IsSecure = false } },
        { HttpActions.SendConfirmAction, new EndpointOptions { Endpoint = SendConfirm, IsAvailable = true, HttpMethod = HttpMethod.Post.Method, IsSecure = false } },
        { HttpActions.RefreshTokenAction, new EndpointOptions { Endpoint = RefreshToken, IsAvailable = true, HttpMethod = HttpMethod.Post.Method, IsSecure = false } },
        { HttpActions.SignOutAction, new EndpointOptions { Endpoint = SignOut, IsAvailable = true, HttpMethod = HttpMethod.Delete.Method, IsSecure = true } },
        { HttpActions.TurnOnMfaAction, new EndpointOptions { Endpoint  = Mfa, IsAvailable = true, HttpMethod = HttpMethod.Post.Method, IsSecure = true } },
        { HttpActions.TurnOffMfaAction, new EndpointOptions { Endpoint  = Mfa, IsAvailable = true, HttpMethod = HttpMethod.Delete.Method, IsSecure = true } },
        { HttpActions.SignInMfaAction, new EndpointOptions { Endpoint = SignInMfa, IsAvailable = true, HttpMethod = HttpMethod.Post.Method, IsSecure = false } },
        { HttpActions.GetDevicesAction, new EndpointOptions { Endpoint = Devices, IsAvailable = true, HttpMethod = HttpMethod.Get.Method, IsSecure = true } },
        { HttpActions.InitAction, new EndpointOptions { Endpoint = Init, IsAvailable = true, HttpMethod = HttpMethod.Post.Method, IsSecure = false } }
    };
}
