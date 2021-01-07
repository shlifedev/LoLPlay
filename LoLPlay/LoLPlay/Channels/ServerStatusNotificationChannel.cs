using Discord;
using Discord.Commands;
using Discord.Rest;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LoLPlay.Channels
{
    public class ServerStatusNotificationChannel : ChannelBase
    {
        public ServerStatusNotificationChannel()
        {
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(OnApplicationQuit); ;
        }

        public async Task OnApplicationQuitAsync()
        {
            await LoLPlayManager.Instance.Log(new Discord.LogMessage(Discord.LogSeverity.Debug, "Quit", "봇 종료전 ServerStatusNotificationChannel에 로그를 남깁니다."));
            foreach (var guild in LoLPlayManager.Instance.Client.Guilds)
            {
                foreach (var channel in guild.Channels)
                {
                    if (channel.Id == this.ID)
                    {
                        var socketTextChannel = channel as SocketTextChannel;
                        await socketTextChannel.SendMessageAsync($"{System.DateTime.Now} - 봇 서버가 종료되었습니다.");
                    }
                }
            }
        }
        public void OnApplicationQuit(object sender, EventArgs e)
        {
            OnApplicationQuitAsync().Wait();
        }

    }
}
