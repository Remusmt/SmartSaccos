using RestSharp;
using System;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace SmartSaccos.ApplicationCore.Services
{
    public class MessageService
    {
        public async Task<bool> SendEmail(
              string emailTo,
              string subject,
              string body
          )
        {
            using (var mailMessage = new MailMessage())
            {
                mailMessage.From = new MailAddress("noreply@nesadisacco.com", "NESADI SACCO");
                mailMessage.To.Add(new MailAddress(emailTo));
                mailMessage.Subject = subject;
                mailMessage.Body = body;
                mailMessage.IsBodyHtml = true;

                using SmtpClient smtpClient = new SmtpClient
                {
                    Host = "mail.nesadisacco.com",
                    Port = 25,
                    EnableSsl = false,
                    Credentials = new NetworkCredential("noreply@nesadisacco.com", "nesadi2020")
                };

                try
                {
                    await smtpClient.SendMailAsync(mailMessage).ConfigureAwait(false);
                }
                catch (SmtpException ex)
                {
                    throw ex;
                    //return false;
                }
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
            catch (SmtpException ex)
            {
                throw new SmtpException(ex.Message);
            }
        }

        private string EncodePassword(string userName, string password)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes($"{userName}:{password}"));
        }
    }
}
