using Discord;
using Discord.Commands;
using Discord.WebSocket;
using LoLPlay.Channels;
using RiotSharp;
using System;
using System.Threading.Tasks;

namespace LoLPlay
{
    /// <summary>
    /// 롤 플레이 매니저, 서버 맞춤형으로 제작되었음
    /// </summary>
    public class LoLPlayManager
    {
        public RiotApi riotAPI;
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
        public DiscordSocketClient Client;
        private CommandHandler _commandHandler;
        public ChannelManager ChannelManager;


      
        public SocketGuild GetGuild()
        {
           return LoLPlayManager.Instance.Client.GetGuild(GlobalConfig.DiscordServerID);
        }


        public async Task OnReady()
        {
            await TeamVsTeamManager.Instance.Initialize();
        }

        public async Task Run()
        {
           
         
            Client = new DiscordSocketClient();
            Client.Log += Log;
            Client.Ready += OnReady;
            //클라이언트 커맨드 핸들러 초기화
            _commandHandler = new CommandHandler(Client, new Discord.Commands.CommandService());
            await _commandHandler.InstallCommandsAsync();

            //채널 매니저 초기화
            ChannelManager = new ChannelManager();
            ChannelManager.AddChannel(GlobalConfig.PartyCreateChannelID, new PartyCreateChannel());
            ChannelManager.AddChannel(GlobalConfig.ServerNotificationChannelID, new ServerStatusNotificationChannel());
            ChannelManager.AddChannel(GlobalConfig.PartyAdvertisingChannelID, new PartyAdvertisingChannel());
            ChannelManager.AddChannel(GlobalConfig.LoLTierVerifyChannelID, new LoLTierVerifyChannel());

            //토큰 불러오기
            var token = System.IO.File.ReadAllText("token.txt");
            //토큰 불러오기
            var apikey = System.IO.File.ReadAllText("riot_apikey.txt");
            riotAPI = RiotApi.GetDevelopmentInstance(apikey);


            //봇 실행
            await Client.LoginAsync(TokenType.Bot, token);
            await Client.StartAsync();

            await Client.SetGameAsync("LoLPlay 관리");  
      
            
            while (true)
            {
                var data = Console.ReadLine();

                var msg = data.Split(' ');
                Console.WriteLine(msg.Length);
                if (msg.Length == 1)
                { 
                    if (data == "help")
                    {
                        Console.WriteLine("quit : 서버종료");
                    }
                    if (data == "quit")
                        System.Environment.Exit(0);
                }
                else if(msg.Length >= 2)
                {
                    if (msg[0] == "chat")
                    {
                        string sendMsg = "";
                        int position = 0;
                        for (int i = 2; i < msg.Length; i++)
                            sendMsg += " " + msg[i]; 

                        await GetGuild().GetTextChannel(ulong.Parse(msg[1])).SendMessageAsync(sendMsg);
                    }
                }
               
            }
            await Task.Delay(-1);
        }
        public Task LogDebug(string src, string content)
        {
           return Log(new LogMessage(LogSeverity.Debug, src, content));
        }
        public Task LogError(string src, string content)
        {
            return Log(new LogMessage(LogSeverity.Error, src, content));
        }
        public Task Log(LogMessage msg)
        { 
            Console.WriteLine(msg.ToString());   
            return Task.CompletedTask;
        }
    }
}
