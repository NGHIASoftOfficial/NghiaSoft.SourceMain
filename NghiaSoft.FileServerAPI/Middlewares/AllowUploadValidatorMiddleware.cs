using NghiaSoft.FileServerAPI.Common.AppConfiguration;

namespace NghiaSoft.FileServerAPI.Middlewares;

public class AllowUploadValidatorMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IConfiguration _configuration;

    public AllowUploadValidatorMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        _next = next;
        _configuration = configuration;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var uploadValidator = _configuration.GetSection(nameof(AllowUploadValidator)).Get<AllowUploadValidator>();
        if (uploadValidator is null) throw new Exception("Please set up AllowUploadOrigins to permit their upload file");
        if (uploadValidator.IsAllowAll)
        {
            await _next(context);
            return;
        }
        
        var baseUrl = $"{context.Request.Scheme}://{context.Request.Host}";
        if (uploadValidator.IsValidate(baseUrl))
        {
            // Call the next delegate/middleware in the pipeline.
            await _next(context);
        }
        else
        {
            // Return a 403 Forbidden response.
            context.Response.StatusCode = 403;
            await context.Response.WriteAsync("You are not allowed to upload files to this server.");
        }
    }
}