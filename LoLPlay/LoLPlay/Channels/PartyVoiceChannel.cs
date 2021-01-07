using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
namespace LoLPlay.Channels
{
    public class PartyVoiceChannel : ChannelBase
    {

        public PartyVoiceChannel()
        {
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(OnApplicationQuit);
            LoLPlayManager.Instance.Log(new Discord.LogMessage(Discord.LogSeverity.Debug, "PartyVoice", "파티 보이스 채널 생성됨"));
            LoLPlayManager.Instance.Client.ChannelUpdated += ChannelUpdated;

        }
        public async Task OnApplicationQuitAsync(object sender, EventArgs e)
        {
            await LoLPlayManager.Instance.Log(new Discord.LogMessage(Discord.LogSeverity.Debug, "Quit", "봇 종료전, 생성된 파티채널 삭제중입니다."));
            foreach (var guild in LoLPlayManager.Instance.Client.Guilds)
            {
                foreach (var channel in guild.Channels)
                {
                    if (channel.Id == this.ID)
                    {
                        await channel.DeleteAsync();
                    }
                }
            }
        }
        public void OnApplicationQuit(object sender, EventArgs e)
        {
            OnApplicationQuitAsync(sender, e).Wait();
        }


        public async Task ChannelUpdated(SocketChannel before, SocketChannel after)
        {
            Console.WriteLine(before.Users.Count);
        }
        public override async Task OnReceivedMsg(SocketUserMessage message, string command, List<string> args)
        {

        }
    }
}
