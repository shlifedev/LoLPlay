using Discord.Commands;
using Discord.WebSocket;
using RiotSharp;
using RiotSharp.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoLPlay.Channels
{
    public class LoLTierVerifyChannel : ChannelBase
    {

        public Dictionary<string, string> tierGroup = new Dictionary<string, string>()
        {
            { "bronze", "브론즈"},  { "silver", "실버"},  { "gold", "골드"},  { "platinum", "플레티넘"},  { "diamond", "다이아몬드" }, { "challenger", "첼린저" }, { "master", "마스터" }, { "grandmaster", "그랜드마스터"}
        };
        public List<string> tierList = new List<string>() { "브론즈", "실버", "골드", "플레티넘", "다이아몬드", "첼린저", "마스터", "그랜드마스터"};
        public LoLTierVerifyChannel()
        {
            AddCommand("!티어인증");
        }

       

        public override async Task OnReceivedMsg(SocketUserMessage message, string command, List<string> args)
        {
            if (command == "!티어인증")
            {
                await message.DeleteAsync();
                SocketCommandContext scc = new SocketCommandContext(LoLPlayManager.Instance.Client, message);
                //generate nickname
                string nickname = null;
                foreach (var value in args) nickname += value;

                var summoner = await LoLPlayManager.Instance.riotAPI.Summoner.GetSummonerByNameAsync(Region.Kr, nickname);
                var league = await LoLPlayManager.Instance.riotAPI.League.GetLeagueEntriesBySummonerAsync(Region.Kr, summoner.Id);

                foreach (var value in league)
                {
                    if (value.QueueType == "RANKED_SOLO_5x5")
                    { 
                        //현재 달고있는 모든 티어 역할 삭제
                        foreach (var tier in tierList)
                        {
                            if (scc.Guild.GetUser(message.Author.Id).Roles.Any(role => role.Name == tier))
                            { 
                                await scc.Guild.GetUser(message.Author.Id).RemoveRoleAsync(scc.Guild.Roles.FirstOrDefault(x => x.Name == tier));
                            }
                        }
                    }
                }

                foreach (var value in league)
                {
                    //솔랭
                    if (value.QueueType == "RANKED_SOLO_5x5")
                    {
                        foreach (var tier in tierGroup)
                        {
                            if (value.Tier.ToLower().Contains(tier.Key))
                            {
                                var role = scc.Guild.Roles.FirstOrDefault(x => x.Name == tier.Value);
                                await scc.Guild.GetUser(message.Author.Id).AddRoleAsync(role);
                            }
                        }
                    }
                    //자랭
                    if (value.QueueType == "RANKED_FLEX_SR")
                    {

                    }
                }

            }
        }
    }
}
