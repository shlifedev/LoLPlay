using Discord.Commands;
using Discord.WebSocket;
using LoLPlay.Channels;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

public class CommandHandler
{
    private readonly DiscordSocketClient client;
    private readonly CommandService _commands;

  
    public CommandHandler(DiscordSocketClient client, CommandService commands)
    {
        _commands = commands;
        this.client = client;
    }

    public async Task InstallCommandsAsync()
    { 
        client.MessageReceived += OnClientMessage; 
        await _commands.AddModulesAsync(assembly: Assembly.GetEntryAssembly(),
                                        services: null);
    }

    /// <summary>
    /// 클라이언트 메세지
    /// </summary> 
    private async Task OnClientMessage(SocketMessage messageParam)
    {
        var message = messageParam as SocketUserMessage;
        if (message == null) return;
        int pos = 0;

        var targetChannelObject = LoLPlay.LoLPlayManager.Instance.ChannelManager.GetChannel(messageParam.Channel.Id);

        //메시지 앞에 !이 달려있지 않고, 자신이 호출된게 아니거나 다른 봇이 호출했다면 취소
        if (!(message.HasCharPrefix('!', ref pos) ||
            message.HasMentionPrefix(client.CurrentUser, ref pos)) ||
                message.Author.IsBot)
        {
            if (targetChannelObject != null && !message.Author.IsBot)
            {
                await targetChannelObject.OnReceivedMsg(message, message.Content, null); 
            }
            return;
        }
 

        if(targetChannelObject != null)
        {
            var content = message.Content;
            var split = content.Split(' ');
            Console.WriteLine(split.Length);
            //인자가 있는 명령어
            if(split.Length != 1 && split.Length != 0)
            {
                var cmdExist = targetChannelObject.CommandExist(split[0]);
                List<string> args = new List<string>(split);
                             args.RemoveAt(0);
                if (cmdExist)
                    await targetChannelObject.OnReceivedMsg(message, split[0], args);
            }
            //인자가 없는 명령어
            else
            {
                var cmdExist = targetChannelObject.CommandExist(content);

                if (cmdExist)
                    await targetChannelObject.OnReceivedMsg(message, content, null);
            } 
      
        }
    }
}