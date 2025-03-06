using CusstomAuth.Core.ErrorHandling.Exceptions;
using CusstomAuth.Core.Extensions;
using CusstomAuth.Core.Responses;
using Microsoft.AspNetCore.Http;
using System.Net.Mime;
using System.Text.Json;

namespace CusstomAuth.Core.ErrorHandling.Extensions;

public static class HttpResponseExtensions
{
    public static async Task WriteAsJsonAsync(this HttpResponse response, int statusCode, string error)
    {
        response.ContentType = MediaTypeNames.Application.Json;
        response.StatusCode = statusCode;
        await response.WriteAsync(JsonSerializer.Serialize(new ApiResponse
        {
            StatusCode = statusCode,
            Error = error,
        }, Settings.Api));
    }

    public static async Task WriteAsJsonAsync(this HttpResponse response, ApiResponse apiResponse)
    {
        response.ContentType = MediaTypeNames.Application.Json;
        response.StatusCode = apiResponse.StatusCode;
        await response.WriteAsync(JsonSerializer.Serialize(apiResponse, Settings.Api));
    }

    public static async Task WriteAsJsonAsync(this HttpResponse response, ApiResponse apiResponse, JsonSerializerOptions jsonSerializerOptions)
    {
        response.ContentType = MediaTypeNames.Application.Json;
        response.StatusCode = apiResponse.StatusCode;
        await response.WriteAsync(JsonSerializer.Serialize(apiResponse, jsonSerializerOptions));
    }

    public static Task WriteAsJsonAsync(this HttpResponse response, HttpResponseException exception)
    {
        return WriteAsJsonAsync(response, exception.StatusCode, exception.Error);
    }
}
