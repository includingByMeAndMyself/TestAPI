using MyNamespace;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Client.Console
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var client = new AuthClient("https://localhost:44371/", new HttpClient());

            var request = new LoginRequest()
            {
                LastName = "Иванов"
            };

            var token = await client.LoginAsync(request);

            System.Console.WriteLine(token);
        }
    }
}
