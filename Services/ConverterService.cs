using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using telegram_spamer.Models;

namespace telegram_spamer.Services
{
    public static class ConverterService
    {
        private const string BaseUrl = "https://api.convertio.co";
        private const string ConvertioToken = "d565a7aec857fc98925bc7eb6a61776f";

        public static async Task<string> ConvertToOpus(string urlMp3)
        {
            var convertModel = new ConvertModel
            {
                ApiKey = ConvertioToken,
                Input = "url",
                File = urlMp3,
                FileName = DateTime.Now.Millisecond.ToString(),
                OutputFormat = "opus"
            };

            var data = new StringContent(JsonConvert.SerializeObject(convertModel), Encoding.UTF8,
                "application/x-www-form-urlencoded");

            var resultLoad = await ApiRequest.Post<ConvertLoadResult>(BaseUrl, "/convert", data);

            var count = 10;
            await Task.Delay(2000);
            while (count != 0)
            {
                var resultStatus =
                    await ApiRequest.Get<ConvertStatusResult>(BaseUrl, $"/convert/{resultLoad.Data.Id}/status");
                if (resultStatus.Data.Step == "finish")
                {
                    var bytes = await ApiRequest.Download(resultStatus.Data.Output.Url);
                    var fileName = $"{Directory.GetCurrentDirectory()}\\{DateTime.Now.Millisecond}.opus";
                    await using var fs =
                        new FileStream(fileName, FileMode.Create, FileAccess.Write);
                    fs.Write(bytes, 0, bytes.Length);

                    return fileName;
                }

                await Task.Delay(3000);
                count--;
            }

            return null;
        }
    }
}