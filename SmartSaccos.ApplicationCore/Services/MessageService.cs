using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using RestSharp;
using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SmartSaccos.ApplicationCore.Services
{
    public class MessageService
    {
        public async Task<bool> SendEmail(
              string emailTo,
              string name,
              string subject,
              string body
          )
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("NESADI SACCO", "noreply@nesadisacco.com"));
            message.To.Add(new MailboxAddress(name, emailTo));
            message.Bcc.Add(new MailboxAddress(name, "registration@nesadisacco.com"));
            message.Subject = subject;
            message.Body = new TextPart(TextFormat.Html) { Text = body };

            try
            {
                using var smtp = new MailKit.Net.Smtp.SmtpClient();

                await smtp.ConnectAsync("mail.nesadisacco.com", 465, SecureSocketOptions.SslOnConnect);
                await smtp.AuthenticateAsync("noreply@nesadisacco.com", "nesadi2020");
                await smtp.SendAsync(message);
                await smtp.DisconnectAsync(true);

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return true;
        }

        public async Task<bool> SendSms(string phoneNumber, string message)
        {
            try
            {
                RestClient restClient = new RestClient("http://107.20.199.106/restapi/sms/1/text/single");
                IRestRequest restRequest = new RestRequest(Method.POST);
                restRequest.AddHeader("accept", "application/json");
                restRequest.AddHeader("content-type", "application/json");
                restRequest.AddHeader("authorization", "Basic " + EncodePassword("kageratea", "K@geratea2019"));
                string body = $"\"from\":\"uwazii\",\"to\":\"[{phoneNumber}]\",\"text\":\"{message}\"";
                restRequest.AddParameter("application/json", "{" + body + "}", ParameterType.RequestBody);

                var response = await restClient.ExecuteAsync(restRequest);
                return response.StatusCode == HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string EncodePassword(string userName, string password)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes($"{userName}:{password}"));
        }
    }
}
