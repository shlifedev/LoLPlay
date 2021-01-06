using Discord;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;

namespace LoLPlay
{
    public class Program
    {

        public static void Main(string[] args)
        {
            LoLPlayManager.Instance.Run().GetAwaiter().GetResult();
        }
         

    }
}
