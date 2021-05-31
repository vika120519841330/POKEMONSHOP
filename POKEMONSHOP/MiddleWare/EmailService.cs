using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
namespace POKEMONSHOP.MiddleWare
{
    public class EmailService : IEmailSender
    {
        private string host;
        private int port;
        private bool enableSSL;
        private string userName;
        private string password;

        public EmailService(string host,
                            int port,
                            bool enableSSL, 
                            string userName, 
                            string password)
        {
            this.host = host;
            this.port = port;
            this.enableSSL = enableSSL;
            this.userName = userName;
            this.password = password;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            using (var client = new SmtpClient())
            {
                client.Host = host;
                client.Port = port;
                client.Credentials = new NetworkCredential(userName, password);
                client.EnableSsl = enableSSL;
                client.UseDefaultCredentials = false;
                try
                {
                    await client.SendMailAsync(new MailMessage
                                                                (
                                                                    from: userName,
                                                                    to: email,
                                                                    subject: subject,
                                                                    body: htmlMessage
                                                                )
                    {
                        IsBodyHtml = true
                    }
                                                );
                }
                catch (Exception ex)
                {
                    throw;
                }

                client.Dispose();
            }
        }

    }
}
