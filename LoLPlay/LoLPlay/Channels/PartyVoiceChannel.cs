using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Threading;

namespace LoLPlay.Channels
{
    public class PartyVoiceChannel : ChannelBase
    {
        CancellationTokenSource cts = new CancellationTokenSource();
        public PartyVoiceChannel()
        {
          
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(OnApplicationQuit);
            LoLPlayManager.Instance.Log(new Discord.LogMessage(Discord.LogSeverity.Debug, "PartyVoice", "파티 보이스 채널 생성됨"));
            LoLPlayManager.Instance.Client.ChannelDestroyed += ChannelDestroyed; 
            _ = Task.Factory.StartNew(AutoChannelDelete, cts.Token);
        }
        
        public async Task AutoChannelDelete()
        {
            //1분이후 인원이 0명인 서버인경우 삭제처리
            System.Threading.Thread.Sleep(6000);

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
        public async Task ChannelDestroyed(SocketChannel c)
        {
            cts.Cancel();
        }
        public async Task OnApplicationQuitAsync(object sender, EventArgs e)
        {
            try
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
            catch(Exception ex)
            {
                await LoLPlayManager.Instance.LogError("PartyVoice", ex.Message);
            }
        }
        public void OnApplicationQuit(object sender, EventArgs e)
        {
            OnApplicationQuitAsync(sender, e).Wait(); 
        } 
    }
}
