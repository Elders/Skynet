using Elders.Skynet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Elders.Skyent.ImdbService.Module
{
    class Program : T800
    {
        static void Main(string[] args)
        {
            new Program().PowerUp(args);
        }

        public void Command(params string[] args)
        {

        }

        public void PowerUp(params string[] args)
        {
            var client = new RestSharp.RestClient("http://www.omdbapi.com");
            var request = new RestSharp.RestRequest("/?t=Terminator&y=&plot=short&r=json", RestSharp.Method.GET);
            while (true)
            {
                var response = client.Execute(request);
                Console.WriteLine(response.Content);
                Console.WriteLine(args);
                Thread.Sleep(4000);
            }
        }

        public void Shutdown(params string[] args)
        {

        }
    }
}
