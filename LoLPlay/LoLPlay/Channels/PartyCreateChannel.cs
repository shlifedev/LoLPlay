using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;

namespace LoLPlay.Channels
{
    public class PartyCreateChannel : ChannelBase
    {
        public PartyCreateChannel()
        { 
            AddCommand("!파티생성");
        }


        public void CreatePartyVoiceChannel(SocketUserMessage receivedData, List<string> args)
        {

        }
        public override void OnReceivedMsg(SocketUserMessage message, string command, List<string> args)
        {
            if(command == "!파티생성")
            {
                CreatePartyVoiceChannel(message, args);
            }
        }
    }
}
