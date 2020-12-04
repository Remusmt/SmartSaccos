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
                mailMessage.From = new MailAddress("smartbookserp@gmail.com", "NESADI SACCO");
                mailMessage.To.Add(new MailAddress(emailTo));
                mailMessage.Subject = subject;
                mailMessage.Body = body;
                mailMessage.IsBodyHtml = true;

                using SmtpClient smtpClient = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    Credentials = new NetworkCredential("smartbookserp@gmail.com", "zkmmbjcqpqzvxpeo")
                };

                try
                {
                    await smtpClient.SendMailAsync(mailMessage).ConfigureAwait(false);
                }
                catch (SmtpException)
                {
                    return false;
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
                string body = $"\"from\":\"uwazii\",\"to\":\"[254{phoneNumber.Substring(phoneNumber.Length - 9, phoneNumber.Length - 1)}]\",\"text\":\"{message}\"";
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
