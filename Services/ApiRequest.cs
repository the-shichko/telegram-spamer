using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace telegram_spamer.Services
{
    public static class ApiRequest
    {
        public static async Task<T> Post<T>(string baseUrl, string url, HttpContent formData) where T : class
        {
            var myHttpClient = new HttpClient();
            var response = await myHttpClient.PostAsync($"{baseUrl}{url}", formData);
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(json);
        }

        public static async Task<T> Get<T>(string baseUrl, string url)
        {
            var myHttpClient = new HttpClient();
            var response = await myHttpClient.GetAsync($"{baseUrl}{url}");
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(json);
        }

        public static async Task<byte[]> Download(string url)
        {
            var myHttpClient = new HttpClient();
            return await myHttpClient.GetByteArrayAsync(url);
        } 
    }
}