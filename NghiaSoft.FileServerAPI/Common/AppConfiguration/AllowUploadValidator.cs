namespace NghiaSoft.FileServerAPI.Common.AppConfiguration;

public class AllowUploadValidator
{
    private string[]? _allowUploadOrigins;

    public string[] AllowUploadOrigins
    {
        get => _allowUploadOrigins ?? Array.Empty<string>();
        set { _allowUploadOrigins = value.Select(x => x.ToLower().TrimEnd('/')).ToArray(); }
    }

    public bool IsAllowAll => AllowUploadOrigins.Any(x => x == "*");

    public bool IsValidate(string baseUrl)
    {
        baseUrl = baseUrl.TrimEnd('/');
        return IsAllowAll || AllowUploadOrigins.Any(x => x == baseUrl);
    }
}