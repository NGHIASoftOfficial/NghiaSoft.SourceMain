using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace NghiaSoft.Core.MailService
{
    public static class MailServiceInstaller
    {
        public static IServiceCollection AddEmailService(this IServiceCollection services, IConfiguration configuration)
        {
            var configSection = configuration.GetSection(nameof(MailSettings));
            services.Configure<MailSettings>(configSection);
            services.AddTransient<IAppMailService, AppMailService>();
            return services;
        }
    }
}