using Discord;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;

namespace LoLPlay
{
    public class Program
    {
        //클라이언트
        private DiscordSocketClient _client;
        private CommandHandler _commandHandler;
        public static void Main(string[] args)
      => new Program().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {

            _client = new DiscordSocketClient(); 
            _client.Log += Log;
            _commandHandler = new CommandHandler(_client, new Discord.Commands.CommandService());
            await _commandHandler.InstallCommandsAsync();


            var token = "Nzk2NDgwODE2Mjk3NDEwNTky.X_YikA.iSHT6sUhgGfNs4mZgC8xwLkfcew";
            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();
             
            await Task.Delay(-1);
        }

        public Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
    }
}
