using Discord;
using Discord.WebSocket;
using LoLPlay.Channels;
using System;
using System.Threading.Tasks;

namespace LoLPlay
{
    public class LoLPlayManager
    {
        public static LoLPlayManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new LoLPlayManager();
                return instance;
            }
        }
        private static LoLPlayManager instance;
        
        
        //클라이언트
        private DiscordSocketClient _client;
        private CommandHandler _commandHandler;
        public ChannelManager ChannelManager;

        public async Task Run()
        {
           
            _client = new DiscordSocketClient();
            _client.Log += Log;

            //클라이언트 커맨드 핸들러 초기화
            _commandHandler = new CommandHandler(_client, new Discord.Commands.CommandService());
            await _commandHandler.InstallCommandsAsync();

            //채널 매니저 초기화
            ChannelManager = new ChannelManager();


            //토큰 불러오기
            var token = System.IO.File.ReadAllText("token.txt");


            //봇 실행
            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync(); 

            //무한 반복
            await Task.Delay(-1);
        }

        public Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
    }
}
