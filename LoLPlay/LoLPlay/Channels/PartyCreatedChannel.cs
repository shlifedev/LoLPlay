using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LoLPlay.Channels
{
    public class PartyCreatedChannel : ChannelBase
    {
        public override async Task OnReceivedMsg(SocketUserMessage message, string command, List<string> args)
        {

        }
    }
}
