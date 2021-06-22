using System;
using System.Threading.Tasks;
using TLSharp;

namespace telegram_spamer.Services
{
    public class TelegramService
    {
        private TelegramClient _client;

        public async Task<TelegramClient> Init()
        {
            try
            {
                _client = new TelegramClient(983781, "afd986c05d86af6e6b7d4d603ba9278f");
                await _client.ConnectAsync();

                if (_client.IsUserAuthorized()) return _client;

                const string phone = "375333085326";
                var hash = await _client.SendCodeRequestAsync(phone);
                var code = ""; //Console.ReadLine();

                try
                {
                    await _client.MakeAuthAsync(phone, hash, code);
                }
                catch (CloudPasswordNeededException)
                {
                    var passwordSetting = await _client.GetPasswordSetting();
                    await _client.MakeAuthWithPasswordAsync(passwordSetting, "Puzahamicrosoft");
                    // Console.WriteLine("This account needs cloud password.");
                }

                return _client;
            }
            catch (Exception e)
            {
                _client.Dispose();
                // Console.WriteLine(e.Message);
                return null;
            }
        }
    }
}