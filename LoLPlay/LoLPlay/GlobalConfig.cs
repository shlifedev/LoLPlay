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
        public static ulong PartyCategoryID = 796457840306683995;    //파티 모집 카테고리
        public static ulong PartyAdvertisingChannelID = 796459008659685398; //파티 홍보 채널
        public static ulong ServerNotificationChannelID = 796533464983404624; // 서버 알림 채널 
        public static ulong LoLTierVerifyChannelID = 796554797930119199; // 롤 티어인증 채널
        public static ulong AdminCommandChannel = 797406750147477524; // 어드민 커맨드 입력 채널

        /* 카테고리 정보*/
        public static ulong PartyCreateChannelID = 796524973744586753; // 파티 생성 채널
        public static ulong TeamVsChannelID = 796459617739472926; // 내전 채널


        /*내전 정보*/
        public static int TeamPlayWaitRoomMaxPlayer = 20; //대기실 인원
        public static int TeamPlayGameRoomMaxPlayer = 10; //게임 인원

    }
}
