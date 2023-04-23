using Dissertation_Project.Model.Infra.Interfaces;
using Newtonsoft.Json.Linq;

namespace Dissertation_Project.Model.Infra.Managers
{
    public class Google_Recaptcha:IGoogle_Recaptcha
    {
        private IConfiguration _configuration;
        public Google_Recaptcha(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<bool> Verify(string googleResponse)
        {
            string sec = _configuration["reCAPTCHA:SECRETKEY"];
            HttpClient httpClient = new HttpClient();
            var result = await httpClient.PostAsync($"https://www.google.com/recaptcha/api/siteverify?secret={sec}&response={googleResponse}", null);
            if (result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                return false;
            }

            string content = result.Content.ReadAsStringAsync().Result;
            dynamic jsonData = JObject.Parse(content);

            if (jsonData.success == "true")
            {
                return true;
            }
            return false;
        }
    }
}
