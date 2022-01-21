using System.Net.Http;
using System.Threading.Tasks;
using AbstractApi.Models;
using Newtonsoft.Json;

namespace AbstractApi
{
    public class AbstractApiClient
    {
        public async Task<bool> Valid(string emailAddress)
        {
            if (!Config.IsActive) return true;
            var fullRequestUrl = $"{Config.Endpoint}?api_key={Config.ApiKey}&email={emailAddress}";
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(fullRequestUrl);
            response.EnsureSuccessStatusCode();
            var res = JsonConvert.DeserializeObject<ValidEmailResult>(await response.Content.ReadAsStringAsync());
            return res != null && res.QualityScore >= Config.MinScore;
        }
    }
}