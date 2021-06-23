using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace telegram_spamer.Services
{
    public class ConverterService
    {
        private const string ConvertioToken = "06ea708c73b68252027134a7b47b0550";

        public async Task<string> ConvertToOpus(string urlMp3)
        {


            var data = new StringContent(json, Encoding.UTF8, "application/json");
            return await ApiRequest.Post<>()
        }
    }
}