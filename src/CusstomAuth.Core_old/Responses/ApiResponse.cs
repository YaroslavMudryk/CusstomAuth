using System.Text.Json.Serialization;

namespace CusstomAuth.Core.Responses;

public class ApiResponse
{
    public int StatusCode { get; set; }
    public string Error { get; set; }
    public string Warning { get; set; }
    public object Data { get; set; }

    public static ApiResponse Success(int statusCode, object data)
    {
        return new ApiResponse { StatusCode = statusCode, Data = data, Error = null };
    }

    public static ApiResponse SuccessWithWarning(int statusCode, object data, string warning)
    {
        return new ApiResponse { StatusCode = statusCode, Data = data, Warning = warning, Error = null };
    }

    public static ApiResponse ValidationFail(Dictionary<string, string[]> validationErrors)
    {
        return new ApiResponse { Error = "Validation errors", Data = validationErrors, StatusCode = 400 };
    }
}

public class ApiResponse<T> : ApiResponse
{
    public new T Data { get; set; }

    public static ApiResponse Ok(T data)
    {
        return new ApiResponse<T>
        {
            Data = data
        };
    }
}