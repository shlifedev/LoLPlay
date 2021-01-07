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
    public class PartyCreateChannel : ChannelBase
    {
 
        /// <summary>
        /// 파티생성시 딜레이 있음
        /// </summary>
        public bool usePartyCreateDelay = false;
        
        /// <summary>
        /// 밀리세컨드 단위, 파티생성 딜레이
        /// </summary>
        public int partyCreateDelayValue = 60000;
       
        /// <summary>
        /// 딜레이 그룹
        /// </summary>
        public Dictionary<ulong, System.DateTime> partyCreateDelay = new Dictionary<ulong, DateTime>();

        /// <summary>
        /// UserVoiceStateUpdated 의 콜백이 동시호출 되는경우를 방지
        /// </summary>
        private HashSet<ulong> deletedVoiceChannels = new HashSet<ulong>();


        /// <summary>
        /// 파티가 생성된경우 나타나는 메세지의 캐시
        /// </summary>
        public List<RestMessage> partyCreateMsgs = new List<RestMessage>();
  
        public PartyCreateChannel()
        {
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(OnApplicationQuit);
            LoLPlayManager.Instance.Client.UserVoiceStateUpdated += UserVoiceStateUpdated;
            AddCommand("!파티생성");
            AddCommand("!Debug");
        }


        public async Task UserVoiceStateUpdated(SocketUser user, SocketVoiceState before, SocketVoiceState after)
        {
            if (before.VoiceChannel != null)
            {
                var userCount = before.VoiceChannel.Users.Count;
                if (userCount == 0)
                {
                    if (before.VoiceChannel != null)
                    { 
                        if (deletedVoiceChannels.Contains(before.VoiceChannel.Id) == false)
                        {
                            try
                            {
                                deletedVoiceChannels.Add(before.VoiceChannel.Id);
                                await before.VoiceChannel.DeleteAsync();
                                await LoLPlayManager.Instance.LogError("PartyVoice", "보이스 채널 삭제 - " + before.VoiceChannel.Name);
                                deletedVoiceChannels.Remove(before.VoiceChannel.Id);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("Exception :: " + e.Message);
                            }
                        }
                    }
                }

            }
        }

        public async Task OnApplicationQuitAsync()
        {
            await LoLPlayManager.Instance.Log(new Discord.LogMessage(Discord.LogSeverity.Debug, "Quit", "봇 종료전, 생성된 봇 파티생성채팅 삭제중입니다."));
            foreach (var msg in partyCreateMsgs)
            {
                await msg.DeleteAsync(); 
            }
        }
        public void OnApplicationQuit(object sender, EventArgs e)
        {
            OnApplicationQuitAsync().Wait();
        }
        /// <summary>
        /// 파티음성채널 생성
        /// </summary>
        /// <param name="receivedData"></param>
        /// <param name="args"></param>
        public async Task CreatePartyVoiceChannel(SocketUserMessage receivedData, List<string> args)
        {
            //기존메세지 삭제
            await receivedData.DeleteAsync(); 

            if(partyCreateDelay.ContainsKey(receivedData.Author.Id) && usePartyCreateDelay == true)
            {
                await Discord.UserExtensions.SendMessageAsync(receivedData.Author, $"파티를 생성 한 후, 60초 후에 다시 시도할 수 있어요!");
                return;
            }
            SocketCommandContext scc = new SocketCommandContext(LoLPlayManager.Instance.Client, receivedData);
            if (args[0].Length < 2)
            {
                await Discord.UserExtensions.SendMessageAsync(receivedData.Author, $"파티 이름은 2글자 이상이어야 합니다!"); 
                return;
            }

            var voiceChannel = await scc.Guild.CreateVoiceChannelAsync(args[0]);

            //채널생성 및 카테고리지정
            await voiceChannel.ModifyAsync(prop =>
            {
                prop.CategoryId = GlobalConfig.PartyCategoryID;
                prop.UserLimit = 5;
            });



            //권한부여 
            await voiceChannel.AddPermissionOverwriteAsync(receivedData.Author, new Discord.OverwritePermissions(manageChannel: PermValue.Allow));
            var invite = await voiceChannel.CreateInviteAsync(86400, 50);

            LoLPlayManager.Instance.ChannelManager.AddChannel(voiceChannel.Id, new PartyVoiceChannel());

            var msg = await scc.Guild.GetTextChannel(ID).SendMessageAsync($"{receivedData.Author.Mention} 님의 <{args[0]}> 파티가 생성되었어요! :warning:(1분안에 미입장시 채널삭제) \n바로입장:{invite.Url}  (해당 메세지는 1분후 삭제됩니다) \n 파티인원 모집은 파티홍보채널에서 해주세요!");
            partyCreateMsgs.Add(msg);

            //60초후 메세지 삭제
            _ = Task.Factory.StartNew(() =>
            {
                try
                { 
                    Thread.Sleep(60000);
                    var task = msg.DeleteAsync();
                    task.Wait();
                    partyCreateMsgs.Remove(msg); 
                }
                catch
                {
                    LoLPlayManager.Instance.LogError("PartyMsg", msg.Content);
                }
            });

            if (usePartyCreateDelay)
            {
                _ = Task.Factory.StartNew(() =>
                {
                    try
                    {
                        partyCreateDelay.Add(receivedData.Author.Id, System.DateTime.Now.AddMilliseconds(partyCreateDelayValue));
                        Thread.Sleep(partyCreateDelayValue);
                        partyCreateDelay.Remove(receivedData.Author.Id);
                    }
                    catch
                    {
                        LoLPlayManager.Instance.LogError("PartyMsg", msg.Content);
                    }
                });
            }

        }
        public override async Task OnReceivedMsg(SocketUserMessage message, string command, List<string> args)
        {
            if (command == "!파티생성")
            {
                await CreatePartyVoiceChannel(message, args);
            }
        }
    }
}
