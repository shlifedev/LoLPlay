using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LoLPlay.Channels
{
    public class PartyCreateChannel : ChannelBase
    {
        public PartyCreateChannel()
        {
            AddCommand("!파티생성");
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
            SocketCommandContext scc = new SocketCommandContext(LoLPlayManager.Instance.Client, receivedData);
            if (args[0].Length < 2)
            {
                await scc.Guild.GetTextChannel(ID).SendMessageAsync($"파티 이름은 2글자 이상이어야 합니다!");
                return;
            }

            var voiceChannel = await scc.Guild.CreateVoiceChannelAsync(args[0]);

            //채널생성 및 카테고리지정
            await voiceChannel.ModifyAsync(prop =>
            {
                prop.CategoryId = 796457840306683995;
                prop.UserLimit = 5;
            });



            //권한부여 
            await voiceChannel.AddPermissionOverwriteAsync(receivedData.Author, new Discord.OverwritePermissions(manageChannel: PermValue.Allow));


            var invite = await voiceChannel.CreateInviteAsync(86400, 50);

            await scc.Guild.GetTextChannel(ID).SendMessageAsync($"{receivedData.Author.Mention} 님의 {args[0]}파티가 생성되었어요! \n바로입장:{invite.Url}  (해당 메세지는 1분후 삭제됩니다) \n 파티인원 모집은 파티홍보채널에서 해주세요!");
        }
        public override async Task OnReceivedMsg(SocketUserMessage message, string command, List<string> args)
        {
            if (command == "!파티생성")
            {
                Console.WriteLine($"파티생성 - {args[0]}");
                await CreatePartyVoiceChannel(message, args);
            }
        }
    }
}
