using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LoLPlay.Channels
{
    public class AdminCommandChannel : ChannelBase
    {
        public AdminCommandChannel()
        {
            AddCommand("!티어조회");
            AppDomain.CurrentDomain.ProcessExit += OnApplicationQuit;
        }

        public async Task InquiryLoLTier(ulong discordID)
        {
            var records = await DB.GetTierVerifyRecordAsync(discordID);
            string tierVerifyLogs = "";
            if (records != null)
            {
                foreach (var value in records)
                    tierVerifyLogs += $"`{value.DateTime}  {value.DiscordNick}({value.DiscordID})  {value.LOLNick}({value.SoloTier})`\n";
            }
            await LoLPlayManager.Instance.GetGuild().GetTextChannel(GlobalConfig.AdminCommandChannel).SendMessageAsync(tierVerifyLogs);
        }
        public override async Task OnReceivedMsg(SocketUserMessage message, string command, List<string> args)
        {
            if (command == "!티어조회")
                await InquiryLoLTier(ulong.Parse(args[0]));
        }

        public override void OnApplicationQuit(object sender, EventArgs e)
        {
            base.OnApplicationQuit(sender, e);
        }

        public override Task OnApplicationQuitAsync(object sender, EventArgs e)
        {
            return base.OnApplicationQuitAsync(sender, e);
        }

    }
}
