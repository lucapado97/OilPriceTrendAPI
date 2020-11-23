using System;
using System.Net;

namespace OilPriceTrend
{
    /// <summary>
    /// main class of the program
    /// </summary>
    class Program
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            var server = new OilServiceLib.HttpWebServer();
            server.Start();
            Console.WriteLine("Enter a char to exit");
            Console.Read();
            server.Stop();
        }
    }
}
