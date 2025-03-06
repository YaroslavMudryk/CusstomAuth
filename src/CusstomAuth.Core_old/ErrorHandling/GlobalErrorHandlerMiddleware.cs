using CusstomAuth.Core.ErrorHandling.Exceptions;
using CusstomAuth.Core.ErrorHandling.Extensions;
using CusstomAuth.Core.Extensions;
using CusstomAuth.Core.Responses;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace CusstomAuth.Core.ErrorHandling;

public class GlobalErrorHandlerMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (ValidationException ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status400BadRequest;

            var validationErrors = ex.Errors
                    .ToLookup(x => x.PropertyName, x => x.ErrorMessage)
                    .ToDictionary(x => x.Key, x => x.ToArray());

            await context.Response.WriteAsJsonAsync(ApiResponse.ValidationFail(validationErrors), Settings.Api);
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
