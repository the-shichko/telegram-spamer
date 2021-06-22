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
        private readonly TelegramClient _client;

        public ComplimentService(TelegramClient client)
        {
            _client = client;
        }

        public async Task Start()
        {
            while (true)
            {
                await SendMessage();
                await Task.Delay(1000 * 60 * 60);
            }
        }

        private async Task SendMessage()
        {
            var result = await _client.GetContactsAsync();

            var user = result.Users
                .Where(absUser => absUser.GetType() == typeof(TLUser))
                .Cast<TLUser>()
                .FirstOrDefault(x => x.Username == "gektorsun");

            var complimentList = await GetCompliments();

            var random = new Random();
            var index = random.Next(0, complimentList.Count);
            if (user != null)
            {
                await _client.SendMessageAsync(new TLInputPeerUser {UserId = user.Id}, complimentList[index] + " 😚");
                // Console.WriteLine($"Send: {complimentList[index] + " 😚"}");
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