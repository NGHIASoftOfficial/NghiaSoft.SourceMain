namespace NghiaSoft.Core.MailService
{
    public class MailRequest
    {
        public MailRequest(string toEmail, string subject, string body)
        {
            ToEmail = toEmail;
            Subject = subject;
            Body = body;
        }

        public string ToEmail { get; }
        public string Subject { get; }
        public string Body { get; }
    }
}