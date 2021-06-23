using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using telegram_spamer.Models;

namespace telegram_spamer.Services
{
    public static class VoiceService
    {
        private const string BaseUrl = "https://apihost.ru";

        public static async Task<string> GetVoice(string message)
        {
            var formContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("from", message),
                new KeyValuePair<string, string>("lang", "ru-RU"),
                new KeyValuePair<string, string>("speaker", "ermilov"),
                new KeyValuePair<string, string>("emotion", "neutral"),
                new KeyValuePair<string, string>("speed", "1.0"),
                new KeyValuePair<string, string>("format", "mp3"),
                new KeyValuePair<string, string>("pitch5", "1.0"),
            });
            var model = await ApiRequest.Post<VoiceModel>(BaseUrl, "d1_030321.php", formContent);
            return model.Fname;
        }
    }
}