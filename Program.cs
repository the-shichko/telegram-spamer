using System.Threading.Tasks;
using telegram_spamer.Services;

namespace telegram_spamer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var telegram = new TelegramService();
            var complimentService = new ComplimentService(await telegram.Init());
            await complimentService.Start();
        }
    }
}