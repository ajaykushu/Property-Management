using Microsoft.Extensions.Options;
using Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Utilities
{
    public class EmailSender : IEmailSender
    {
        private readonly IOptions<EmailConfigurationModel> _smtpconfig;

        public EmailSender(IOptions<EmailConfigurationModel> smtpconfig)
        {
            _smtpconfig = smtpconfig;
        }

        public async Task SendAsync(string recieveremail, string subject, string body, bool isbodyHTML)
        {
            try
            {
                var client = BuildClient();
                var mailMessage = BuildMailMessage(
                    body,
                    new List<string>() { recieveremail },
                    subject,
                    isbodyHTML);
                client.SendCompleted += (s, e) =>
                {
                    client.Dispose();
                    mailMessage.Dispose();
                };
                await client.SendMailAsync(mailMessage);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task SendAsync(List<string> recieveremail, string subject, string body, bool isbodyHTML)
        {
            try
            {
                var client = BuildClient();
                var mailMessage = BuildMailMessage(body, recieveremail, subject, isbodyHTML);
                client.SendCompleted += (s, e) =>
                {
                    client.Dispose();
                    mailMessage.Dispose();
                };
                await client.SendMailAsync(mailMessage);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public SmtpClient BuildClient()
        {
            SmtpClient client = new SmtpClient(_smtpconfig.Value.SMTPServerAddress, _smtpconfig.Value.Port)
            {
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_smtpconfig.Value.UserName, _smtpconfig.Value.Password),
                EnableSsl = true
            };
            return client;
        }

        public MailMessage BuildMailMessage(string body, List<string> receiverEmail, string subject, bool isHTML)
        {
            MailMessage mailMessage = new MailMessage
            {
                From = new MailAddress(_smtpconfig.Value.From),
                Body = body,
                Subject = subject,
                IsBodyHtml = isHTML,
                Priority = MailPriority.High
            };
            foreach (string emailAddress in receiverEmail)
                mailMessage.To.Add(emailAddress);

            return mailMessage;
        }
    }
}