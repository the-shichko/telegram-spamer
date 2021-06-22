using System;
using System.Threading.Tasks;
using telegram_spamer.Services;

namespace telegram_spamer
{
    class Program
    {
        static void Main(string[] args)
        {
            _ = new ComplimentService();
            Console.WriteLine("Process started");
            Console.ReadLine();
        }
    }
}