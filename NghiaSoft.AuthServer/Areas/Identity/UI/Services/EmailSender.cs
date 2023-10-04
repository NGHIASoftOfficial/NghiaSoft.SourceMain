using Microsoft.AspNetCore.Identity.UI.Services;
using NghiaSoft.Core.MailService;

namespace NghiaSoft.AuthServer.Areas.Identity.UI.Services;

public class EmailSender : IEmailSender
{
    private readonly IAppMailService _mailService;

    public EmailSender(IAppMailService mailService)
    {
        _mailService = mailService;
    }

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        var mr = new MailRequest(email, subject, htmlMessage);
        await _mailService.SendEmailAsync(mr);
    }
}