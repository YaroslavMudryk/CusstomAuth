namespace CusstomAuth.Core.Responses;

public class ApiResponse
{
    public int StatusCode { get; set; }
    public string Error { get; set; } = default!;
    public string Warning { get; set; } = default!;
    public object Data { get; set; } = default!;

    public static ApiResponse Success(int statusCode, object data)
    {
        return new ApiResponse { StatusCode = statusCode, Data = data, Error = default! };
    }

    public static ApiResponse SuccessWithWarning(int statusCode, object data, string warning)
    {
        return new ApiResponse { StatusCode = statusCode, Data = data, Warning = warning, Error = default! };
    }

    public static ApiResponse ValidationFail(Dictionary<string, List<string>> validationErrors)
    {
        return new ApiResponse { Error = "Validation errors", Data = validationErrors, StatusCode = 400 };
    }
}

public class ApiResponse<T> : ApiResponse
{
    public new T Data { get; set; } = default!;

    public static ApiResponse Ok(T data)
    {
        return new ApiResponse<T>
        {
            Data = data
        };
    }
}
