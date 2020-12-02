using System;
using System.Security.Cryptography.X509Certificates;
namespace Genshin.KeyStealer
{
    class Program
    {
        static void Main(string[] args)
        {
            var stealer = new AuthKeyStealer();
            stealer.Start("127.0.0.1", 3500);
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
            stealer.Stop();
        }
    }
}