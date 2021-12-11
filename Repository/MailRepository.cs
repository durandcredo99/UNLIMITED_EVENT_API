using Contracts;
using Entities.Models;
using Entities.RequestFeatures;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class MailRepository : IMailRepository
    {
        EmailSettings _emailSettings = null;

        public MailRepository(IOptions<EmailSettings> options)
        {
            _emailSettings = options.Value;
        }

     
        public async Task SendEmailAsync(EmailModel emailData)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress(_emailSettings.Name, _emailSettings.EmailId));

            var emailTo = new MailboxAddress(emailData.ToName, emailData.ToEmail);
            emailMessage.To.Add(emailTo);

            emailMessage.Subject = emailData.Subject;

            BodyBuilder emailBodyBuilder = new BodyBuilder();
            emailBodyBuilder.TextBody = emailData.Body;
            emailMessage.Body = emailBodyBuilder.ToMessageBody();

            SmtpClient emailClient = new SmtpClient();
            await emailClient.ConnectAsync(_emailSettings.Host, _emailSettings.Port, _emailSettings.UseSSL);

            await emailClient.AuthenticateAsync(_emailSettings.EmailId, _emailSettings.Password);
            await emailClient.SendAsync(emailMessage);
            await emailClient.DisconnectAsync(true);
            emailClient.Dispose();
        }

        /*
        
        public async Task SendEmailAsync(EmailModel emailData)
        {
            var message = new MimeMessage();

            //add the sender info that will appear in the email message
            var emailAddress = "amstrong@aun.bj";
            var password = "Vre7GzTzdAd4vjt@";
            //var emailAddress = "amstrong.smtp@gmail.com";
            //var password = "uNhZzeUYtjW8B9j";

            message.From.Add(new MailboxAddress("Tester",emailAddress));

            //add the receiver email address
            message.To.Add(MailboxAddress.Parse("stephane.adjakotan@gmail.com"));

            //add the message subject
            message.Subject = "le second";

            message.Body = new TextPart("plain")
            {
                Text = @"Yes,
                Hello!.
                you are fantastique Stéphane"
            };

            SmtpClient client = new SmtpClient();

            await client.ConnectAsync("mail5015.site4now.net", 465, true);

            await client.AuthenticateAsync(emailAddress, password);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
            client.Dispose();
        }
        */
    }
}

