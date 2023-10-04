using System.Threading.Tasks;

namespace NghiaSoft.Core.MailService
{
    public interface IAppMailService
    {
        Task SendEmailAsync(MailRequest mailRequest);
    }
}