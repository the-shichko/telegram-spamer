using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using TLSchema;
using TLSharp;
using TLSharp.Utils;

#pragma warning disable 618

namespace telegram_spamer.Services
{
    public class TelegramService
    {
        private const string Token = "1774571118:AAEtXLBEDn1_H8wlp5PI9dUP289hdIDrLtE";
        private static ITelegramBotClient _botClient;
        private static string _hash;
        private static string _phone;
        private static Func<Task> _spamMethod;

        public static async Task<TelegramClient> Init(Func<Task> runFunc)
        {
            try
            {
                _spamMethod = runFunc;
                _botClient = new TelegramBotClient(Token);
                _botClient.StartReceiving();
                _botClient.OnMessage += OnBotMessage;

                TelegramClient = new TelegramClient(983781, "afd986c05d86af6e6b7d4d603ba9278f");
                await TelegramClient.ConnectAsync();

                return TelegramClient;
            }
            catch (Exception e)
            {
                TelegramClient.Dispose();
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public static async Task SentMessage(string message)
        {
            var result = await TelegramClient.GetContactsAsync();

            var user = result.Users
                .Where(absUser => absUser.GetType() == typeof(TLUser))
                .Cast<TLUser>()
                .FirstOrDefault(x => x.Username == "gektorsun");

            if (user != null)
            {
                await TelegramClient.SendMessageAsync(new TLInputPeerUser {UserId = 337383405}, message + " 😚");
                Console.WriteLine($"Send: {message + " 😚"}");
            }
        }

        public static async Task SentAudio(string message)
        {
            var result = await TelegramClient.GetContactsAsync();

            var user = result.Users
                .Where(absUser => absUser.GetType() == typeof(TLUser))
                .Cast<TLUser>()
                .FirstOrDefault(x => x.Username == "gektorsun");

            var attr = new TLVector<TLAbsDocumentAttribute>()
            {
                new TLDocumentAttributeAudio {Voice = true},
            };

            if (user != null)
            {
                var file = new StreamReader(@"C:\Users\pasha\OneDrive\trash\Обэме (донат Братишкина)_uAZk.ogg");
                var fileResult = (TLInputFile) await TelegramClient.UploadFile("test.ogg", file);
                await TelegramClient.SendUploadedDocument(new TLInputPeerUser {UserId = 337383405}, fileResult,
                    "", "audio/ogg", attr);
                Console.WriteLine($"Send: {message + " 😚"}");
            }
        }

        private static async Task SentBotMessage(long chatId, string message)
        {
            await _botClient.SendTextMessageAsync(chatId, message);
        }

        private static async void OnBotMessage(object sender, MessageEventArgs e)
        {
            try
            {
                var chatId = e.Message.Chat.Id;
                var text = e.Message.Text;
                switch (text)
                {
                    case { } command when command.StartsWith("/user"):
                        await SentBotMessage(chatId, $"Status: IsUserAuthorized - {TelegramClient.IsUserAuthorized()}");
                        break;
                    case { } command when command.StartsWith("/login"):
                        await SentBotMessage(chatId, "Print phone number");
                        break;
                    case { } command when command.StartsWith("/phone"):
                        var argsPhone = command.Split(" ").ToList();
                        _phone = argsPhone[1];
                        _hash = await TelegramClient.SendCodeRequestAsync(_phone);
                        await SentBotMessage(chatId, "Print telegram code");
                        break;
                    case { } command when command.StartsWith("code"):
                        var argsCode = command.Split(" ").ToList();
                        try
                        {
                            await TelegramClient.MakeAuthAsync(_phone, _hash,
                                argsCode[1].Replace("\"", ""));
                        }
                        catch (CloudPasswordNeededException)
                        {
                            await SentBotMessage(chatId, "Print telegram password");
                        }

                        break;
                    case { } command when command.StartsWith("/password"):
                        var argsPassword = command.Split(" ").ToList();
                        var passwordSetting = await TelegramClient.GetPasswordSetting();
                        await TelegramClient.MakeAuthWithPasswordAsync(passwordSetting, argsPassword[1]);
                        await SentBotMessage(chatId, $"Status: IsUserAuthorized - {TelegramClient.IsUserAuthorized()}");
                        break;
                    case { } command when command.StartsWith("/run"):
                        RunScheduler();
                        await SentBotMessage(chatId, $"Completed run");
                        break;
                    case { } command when command.StartsWith("/stop"):
                        StopScheduler();
                        await SentBotMessage(chatId, $"Completed stop");
                        break;
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }

        private static void RunScheduler()
        {
            Scheduler.IntervalInHours(1, async () => await _spamMethod.Invoke());
        }

        private static void StopScheduler()
        {
            Scheduler.StopTasks();
        }

        private static TelegramClient TelegramClient { get; set; }

        public bool IsRunning { get; set; }
    }
}