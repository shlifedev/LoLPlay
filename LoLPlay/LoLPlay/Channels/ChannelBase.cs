using System;
using System.Collections.Generic;
using System.Text;

namespace LoLPlay.Channels
{
    public class ChannelBase
    { 
        public ulong ID;
        public void Init(ulong channelId)
        {
            this.ID = channelId;
        }
        public void CreateChannel()
        {

        }
    }
}
