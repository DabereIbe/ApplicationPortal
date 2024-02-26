using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace ApplicationPortal.Utilities
{
    public class MailKitEmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;
        public MailkitDetails _mailKitDetails;
        public MailKitEmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            return sendMessage(email, subject, htmlMessage);
        }

        public Task sendMessage(string email, string subject, string htmlMessage)
        {
            _mailKitDetails = _configuration.GetSection("MailKit").Get<MailkitDetails>();

            MimeMessage message = new MimeMessage();
            //Sender's Email
            message.From.Add(new MailboxAddress(_mailKitDetails.SenderName, _mailKitDetails.SenderEmail));

            //Receiver's Email
            message.To.Add(MailboxAddress.Parse(email));

            //Message Subject
            message.Subject = subject;

            //Message Body
            message.Body = new TextPart("html")
            {
                Text = htmlMessage
            };


            SmtpClient client = new SmtpClient();

            try
            {
                client.Connect(_mailKitDetails.SmtpServer, _mailKitDetails.PortNumber, _mailKitDetails.TrustedConnection);
                client.Authenticate(_mailKitDetails.Username, _mailKitDetails.Password);
                client.Send(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                client.Disconnect(true);
                client.Dispose();
            }

            return Task.FromResult(true);
        }
    }
}