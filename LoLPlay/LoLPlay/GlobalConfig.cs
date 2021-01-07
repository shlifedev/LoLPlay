using System;
using System.Collections.Generic;
using System.Text;

namespace LoLPlay
{
    /// <summary>
    /// 추후 ID의 관리는 봇에게 입력시켜 맡겨야함.
    /// </summary>
    public static class GlobalConfig
    {
        /* 디스코드 서버 ID */
        public static ulong DiscordServerID = 796455016986312735;

        /* 채널 정보*/
        public static ulong PartyCategoryID = 796457840306683995; 
        public static ulong PartyAdvertisingChannelID = 796459008659685398;
        public static ulong ServerNotificationChannelID = 796533464983404624;
        public static ulong LoLTierVerifyChannelID = 796554797930119199;

        /* 카테고리 정보*/
        public static ulong PartyCreateChannelID = 796524973744586753;
        public static ulong TeamVsChannelID = 796459617739472926;

    }
}
