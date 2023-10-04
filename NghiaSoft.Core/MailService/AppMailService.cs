using System.IO;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace NghiaSoft.Core.MailService
{
    public class AppMailService : IAppMailService
    {
        private readonly MailSettings _mailSettings;

        public AppMailService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }
        
        public async Task SendEmailAsync(MailRequest mailRequest)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
            email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
            email.Subject = mailRequest.Subject;
            var builder = new BodyBuilder
            {
                // }
                //     }
                //         }
                //             builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
                //             }
                //                 fileBytes = ms.ToArray();
                //                 file.CopyTo(ms);
                //             {
                //             using (var ms = new MemoryStream())
                //         {
                //         if (file.Length > 0)
                //     {
                //     foreach (var file in mailRequest.Attachments)
                //     byte[] fileBytes;
                // {
                // if (mailRequest.Attachments != null)
                HtmlBody = mailRequest.Body
            };
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_mailSettings.Mail, _mailSettings.Password);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}