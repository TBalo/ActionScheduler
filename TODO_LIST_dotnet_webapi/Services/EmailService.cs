using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TODO_LIST.Services
{
    public class EmailService
    {
        private readonly string apiKey = "A91496633F58FFA1C8923E777B44B008A22471D5E0089838645ABD0C3BFABB060BD18AA1870396AE0C674C6A0EE00E2F";  
        private readonly string fromEmail = "tbalog426@gmail.com";  

        public async Task<bool> SendPasswordResetEmail(string toEmail, string resetLink)
        {
            var httpClient = new HttpClient();
            var url = "https://api.elasticemail.com/v2/email/send";

            var payload = new
            {
                apiKey = apiKey,
                from = fromEmail,
                to = toEmail,
                subject = "Password Reset Request",
                bodyHtml = $"<p>Click <a href='{resetLink}'>here</a> to reset your password.</p>",
                isTransactional = true
            };

            var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync(url, content);
            return response.IsSuccessStatusCode;
        }
    }
}
