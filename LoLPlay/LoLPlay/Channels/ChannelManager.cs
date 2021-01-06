using System;
using System.Collections.Generic;
using System.Text;

namespace LoLPlay.Channels
{
    public class ChannelManager
    {
        public Dictionary<ulong, ChannelBase> channelMap = new Dictionary<ulong, ChannelBase>();
        public List<ChannelBase> channels = new List<ChannelBase>();

        public void AddChannel(ulong id, ChannelBase channelObject)
        {
            if (channelMap.ContainsKey(id) == false)
            {
                channelMap.Add(id, channelObject);
                channels.Add(channelObject);
            }
        }
    }
}
