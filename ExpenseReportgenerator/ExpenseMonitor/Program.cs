using System;
using System.Net;
using System.Threading.Tasks;

namespace ExpenseMonitor
{
    class Program
    {
        static void Main(string[] args)
        {
            callWebApi().Wait();
            Console.WriteLine("Hello World!");
            Console.ReadLine();
        }

        static private async Task callWebApi()
        {
            WebResponse response = await WebRequest
                .Create("http://www.google.com")
                .GetResponseAsync()
                .ConfigureAwait(false); 
            Console.WriteLine(response.ToString());
            
        }

    }


}
