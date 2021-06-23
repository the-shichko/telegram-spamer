using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using TLSchema;
using TLSharp;

namespace telegram_spamer.Services
{
    public class ComplimentService
    {
        private readonly TelegramService _telegramService = new TelegramService();

        public ComplimentService()
        {
            TelegramService.Init(SentMessage).GetAwaiter().GetResult();

            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
        }

        private async Task SentMessage()
        {
            try
            {
                var complimentList = await GetCompliments();
                var random = new Random();
                var index = random.Next(0, complimentList.Count);
                await TelegramService.SentAudio(complimentList[index]);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private static async Task<List<string>> GetCompliments()
        {
            var page = await Parser.ParsePage(
                "https://mensby.com/women/relations/krasivye-komplimenty-devushke-o-ee-krasote-555-komplimentov-devushke");
            return Parser.ParseText(page, "p", new Regex(@"^\d+. ")).Select(x => Regex.Replace(x, @"^\d+. ", ""))
                .ToList();
        }
    }
}