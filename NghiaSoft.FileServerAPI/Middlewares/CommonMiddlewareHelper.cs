namespace NghiaSoft.FileServerAPI.Middlewares;

public static class CommonMiddlewareHelper
{
    public static IApplicationBuilder UseAllowUploadValidatorMiddleware(
        this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<AllowUploadValidatorMiddleware>();
    }
}