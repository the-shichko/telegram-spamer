using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TLSchema;
using TLSharp;

namespace telegram_spamer.Services
{
    public class ComplimentService
    {
        private readonly TelegramService _telegramService = new TelegramService();

        public ComplimentService()
        {
            _telegramService.Init(SentMessage).GetAwaiter().GetResult();
        }
        
        private async Task SentMessage()
        {
            var complimentList = await GetCompliments();
            var random = new Random();
            var index = random.Next(0, complimentList.Count);
            await _telegramService.SentMessage(complimentList[index]);
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