namespace NghiaSoft.Core.IdentityServer
{
    public class ApiClientConnector
    {
        public string ClientId { get; set; }
        public string Authority { get; set; }
        public string AuthorizationUrl { get; set; }
        public string TokenUrl { get; set; }
        public string RefreshUrl { get; set; }
        public string Audience { get; set; }

        public string AbsoluteAuthorizationUrl => Authority.TrimEnd('/') + '/' + AuthorizationUrl.TrimStart('/');
        public string AbsoluteTokenUrl => Authority.TrimEnd('/') + '/' + TokenUrl.TrimStart('/');
        public string AbsoluteRefreshUrl => Authority.TrimEnd('/') + '/' + RefreshUrl.TrimStart('/');

        public ApiClientConnectorScope[] Scopes { get; set; }
    }

    public class ApiClientConnectorScope
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsDefault { get; set; }
    }
}