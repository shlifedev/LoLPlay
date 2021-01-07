﻿using Discord;
using Discord.Commands;
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
        public DiscordSocketClient Client;
        private CommandHandler _commandHandler;
        public ChannelManager ChannelManager;


      
        public async Task Run()
        {
           
            Client = new DiscordSocketClient();
            Client.Log += Log;

            //클라이언트 커맨드 핸들러 초기화
            _commandHandler = new CommandHandler(Client, new Discord.Commands.CommandService());
            await _commandHandler.InstallCommandsAsync();

            //채널 매니저 초기화
            ChannelManager = new ChannelManager();
            ChannelManager.AddChannel(796524973744586753, new PartyCreateChannel());
            ChannelManager.AddChannel(796533464983404624, new ServerStatusNotificationChannel());

            //토큰 불러오기
            var token = System.IO.File.ReadAllText("token.txt");


            //봇 실행
            await Client.LoginAsync(TokenType.Bot, token);
            await Client.StartAsync();
       
            await Client.SetGameAsync("LoLPlay 관리");
            while (true)
            {
                var data = Console.ReadLine();
                if(data == "A")
                { 
                   
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
