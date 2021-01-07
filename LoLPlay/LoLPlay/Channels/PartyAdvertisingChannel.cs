using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Discord.Commands;

namespace LoLPlay.Channels
{
    public class PartyAdvertisingChannel : ChannelBase
    {

        public PartyAdvertisingChannel()
        {
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(OnApplicationQuit);
        }

        public override async Task OnReceivedMsg(SocketUserMessage message, string command, List<string> args)
        {
            //메세지삭제
            await message.DeleteAsync(); 
            var author = message.Author; 
            SocketCommandContext scc = new SocketCommandContext(LoLPlayManager.Instance.Client, message); 

            //입장채널
            var joinedChannel = scc.Guild.GetUser(author.Id).VoiceChannel; 
            if(joinedChannel != null)
            {
                var invite = await joinedChannel.CreateInviteAsync();
                var url = invite.Url;
                var msg = await scc.Guild.GetTextChannel(ID).SendMessageAsync($"{command} {url} \n 해당메세지는 10분후 자동으로 삭제됩니다");
            }
            else
            {
                await Discord.UserExtensions.SendMessageAsync(message.Author, $"파티생성 채널에서 파티 생성후, 음성 채팅방에 입장한 상태에서만 파티를 홍보할 수 있습니다! \n `삭제된 메세지 : {command}`"); 
            }
       

            
        }

        public async Task OnApplicationQuitAsync(object sender, EventArgs e)
        {
            await LoLPlayManager.Instance.Log(new Discord.LogMessage(Discord.LogSeverity.Debug, "Quit", "파티홍보 채널 채팅기록 클리어중"));
           
        }
        public void OnApplicationQuit(object sender, EventArgs e)
        {
            OnApplicationQuitAsync(sender, e).Wait(); 
        } 
    }
}
