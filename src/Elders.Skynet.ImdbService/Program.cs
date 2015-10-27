using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Elders.Skyent.ImdbService
{
    class Program
    {
        static void Main(string[] args)
        {
            //http://www.omdbapi.com/?t=Frozen&y=&plot=short&r=json

            var client = new RestSharp.RestClient("http://www.omdbapi.com");
            var request = new RestSharp.RestRequest("/?t=Terminator&y=&plot=short&r=json", RestSharp.Method.GET);
            while (true)
            {
                var response = client.Execute(request);
                Console.WriteLine(response.Content);
                Thread.Sleep(4000);
            }
        }
    }
}
