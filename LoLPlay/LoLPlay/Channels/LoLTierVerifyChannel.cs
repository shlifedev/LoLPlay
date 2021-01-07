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
         
        public LoLTierVerifyChannel()
        {  
              AddCommand("!티어인증");
        }
         
        public override async Task OnReceivedMsg(SocketUserMessage message, string command, List<string> args)
        {
           if(command == "!티어인증")
            {
                await message.DeleteAsync();
                SocketCommandContext scc = new SocketCommandContext(LoLPlayManager.Instance.Client, message);
                //generate nickname
                string nickname = null;
                foreach(var value in args) nickname += value;
                 
                var summoner = await LoLPlayManager.Instance.riotAPI.Summoner.GetSummonerByNameAsync(Region.Kr, nickname);
                Console.WriteLine(summoner.Id);
                var league = await LoLPlayManager.Instance.riotAPI.League.GetLeagueEntriesBySummonerAsync(Region.Kr, summoner.Id);
                
                foreach (var value in league)
                { 
                    if (scc.Guild.GetUser(message.Author.Id).Roles.Any(role => role.Name == "브론즈"))
                        await scc.Guild.GetUser(message.Author.Id).RemoveRoleAsync(scc.Guild.Roles.FirstOrDefault(x => x.Name == "브론즈")); 
                    if (scc.Guild.GetUser(message.Author.Id).Roles.Any(role => role.Name == "실버"))
                        await scc.Guild.GetUser(message.Author.Id).RemoveRoleAsync(scc.Guild.Roles.FirstOrDefault(x => x.Name == "실버"));
                    if (scc.Guild.GetUser(message.Author.Id).Roles.Any(role => role.Name == "골드"))
                        await scc.Guild.GetUser(message.Author.Id).RemoveRoleAsync(scc.Guild.Roles.FirstOrDefault(x => x.Name == "골드"));
                    if (scc.Guild.GetUser(message.Author.Id).Roles.Any(role => role.Name == "플레티넘"))
                        await scc.Guild.GetUser(message.Author.Id).RemoveRoleAsync(scc.Guild.Roles.FirstOrDefault(x => x.Name == "플레티넘"));
                    if (scc.Guild.GetUser(message.Author.Id).Roles.Any(role => role.Name == "다이아몬드"))
                        await scc.Guild.GetUser(message.Author.Id).RemoveRoleAsync(scc.Guild.Roles.FirstOrDefault(x => x.Name == "다이아몬드"));
                    if (scc.Guild.GetUser(message.Author.Id).Roles.Any(role => role.Name == "마스터"))
                        await scc.Guild.GetUser(message.Author.Id).RemoveRoleAsync(scc.Guild.Roles.FirstOrDefault(x => x.Name == "마스터"));
                    if (scc.Guild.GetUser(message.Author.Id).Roles.Any(role => role.Name == "그랜드마스터"))
                        await scc.Guild.GetUser(message.Author.Id).RemoveRoleAsync(scc.Guild.Roles.FirstOrDefault(x => x.Name == "그랜드마스터")); 
                    if (scc.Guild.GetUser(message.Author.Id).Roles.Any(role => role.Name == "첼린저"))
                        await scc.Guild.GetUser(message.Author.Id).RemoveRoleAsync(scc.Guild.Roles.FirstOrDefault(x => x.Name == "첼린저"));

                    //솔랭
                    if (value.QueueType == "RANKED_SOLO_5x5")
                    {
                        if (value.Tier.ToLower().Contains("bronze"))
                        {
                            var role = scc.Guild.Roles.FirstOrDefault(x => x.Name == "브론즈");
                            await scc.Guild.GetUser(message.Author.Id).AddRoleAsync(role);
                        }
                        if (value.Tier.ToLower().Contains("silver"))
                        {
                            var role = scc.Guild.Roles.FirstOrDefault(x => x.Name == "실버");
                            await scc.Guild.GetUser(message.Author.Id).AddRoleAsync(role);
                        }
                        if (value.Tier.ToLower().Contains("gold"))
                        {
                            var role = scc.Guild.Roles.FirstOrDefault(x => x.Name == "골드");
                            await scc.Guild.GetUser(message.Author.Id).AddRoleAsync(role);
                        }
                        if (value.Tier.ToLower().Contains("platinum"))
                        {
                            var role = scc.Guild.Roles.FirstOrDefault(x => x.Name == "플레티넘");
                            await scc.Guild.GetUser(message.Author.Id).AddRoleAsync(role);
                        }
                        if (value.Tier.ToLower().Contains("diamond"))
                        {
                            var role = scc.Guild.Roles.FirstOrDefault(x => x.Name == "다이아몬드");
                            await scc.Guild.GetUser(message.Author.Id).AddRoleAsync(role);
                        }
                        if (value.Tier.ToLower().Contains("master"))
                        {
                            var role = scc.Guild.Roles.FirstOrDefault(x => x.Name == "마스터");
                            await scc.Guild.GetUser(message.Author.Id).AddRoleAsync(role);
                        }
                        if (value.Tier.ToLower().Contains("grandmaster"))
                        {
                            var role = scc.Guild.Roles.FirstOrDefault(x => x.Name == "그랜드마스터");
                            await scc.Guild.GetUser(message.Author.Id).AddRoleAsync(role);
                        }
                        if (value.Tier.ToLower().Contains("challenger"))
                        {
                            var role = scc.Guild.Roles.FirstOrDefault(x => x.Name == "첼린저");
                            await scc.Guild.GetUser(message.Author.Id).AddRoleAsync(role);

                          
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
