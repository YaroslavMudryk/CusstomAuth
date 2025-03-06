using CusstomAuth.Core.ErrorHandling.Exceptions;
using CusstomAuth.Core.Extensions;
using CusstomAuth.Core.Responses;
using Microsoft.AspNetCore.Http;

namespace CusstomAuth.Core.ErrorHandling;

public class IdentityErrorHandlerMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (FailedValidationException ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsJsonAsync(ApiResponse.ValidationFail(ex.ValidationErrors), Settings.Api);
        }
        catch (HttpResponseException ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = ex.StatusCode;
            await context.Response.WriteAsJsonAsync(ex);
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}
