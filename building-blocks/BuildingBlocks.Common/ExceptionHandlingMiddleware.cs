using System;
using System.Net;
using System.Threading.Tasks;
using System.Text.Json;           
using System.Threading.Tasks;     
using Microsoft.AspNetCore.Http;  
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (DomainException ex)
        {
            context.Response.StatusCode = ex.Code switch
            {
                ErrorCodes.NotFound => (int)HttpStatusCode.NotFound,
                ErrorCodes.ValidationError => (int)HttpStatusCode.BadRequest,
                ErrorCodes.Unauthorized => (int)HttpStatusCode.Unauthorized,
                ErrorCodes.Forbidden => (int)HttpStatusCode.Forbidden,
                ErrorCodes.Conflict => (int)HttpStatusCode.Conflict,
                _ => (int)HttpStatusCode.InternalServerError
            };

            context.Response.ContentType = "application/json";
            var response = ApiResponse<string>.Fail(ex.Message);
            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";
            var response = ApiResponse<string>.Fail("Unexpected error: " + ex.Message);
            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
