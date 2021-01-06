using Discord.Commands;
using Discord.WebSocket;
using System;
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


        //메시지 앞에 !이 달려있지 않고, 자신이 호출된게 아니거나 다른 봇이 호출했다면 취소
        if (!(message.HasCharPrefix('!', ref pos) ||
            message.HasMentionPrefix(client.CurrentUser, ref pos)) ||
                message.Author.IsBot)
            return;

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("Command - " + messageParam.Content);
        Console.ForegroundColor = ConsoleColor.White;
    }
}