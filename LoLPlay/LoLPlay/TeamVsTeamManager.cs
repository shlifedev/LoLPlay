using Discord.Rest;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LoLPlay
{
    /// <summary>
    /// 롤내전
    /// </summary>
    public class TeamVsTeamManager
    {
        public static TeamVsTeamManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new TeamVsTeamManager();
                return instance;
            }
        }
        private static TeamVsTeamManager instance;
       
        public List<string> TvTChannels = new List<string>() {
            "A", "B", "C", "D"
        }; 

        private string _cfgChannelSuffix = "조";
        private string _cfgWaitRoomName = "대기실";
        private string _cfgBlueTeam = "1팀";
        private string _cfgRedTeam = "2팀";

        public string GetWaitRoomName(string channelName) => $"{channelName}{_cfgChannelSuffix} {_cfgWaitRoomName}";
        public string GetBlueTeamName(string channelName) => $"{channelName}{_cfgChannelSuffix} {_cfgBlueTeam}";
        public string GetRedTeamName(string channelName) => $"{channelName}{_cfgChannelSuffix} {_cfgRedTeam}";

        public List<RestVoiceChannel> createdTvTChannels = new List<RestVoiceChannel>();

        public async Task OnApplicationQuitAsync(object sender, EventArgs e)
        {
            try
            {
                foreach (var value in createdTvTChannels)
                {
                    await value.DeleteAsync();
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        public void OnApplicationQuit(object sender, EventArgs e)
        {
            OnApplicationQuitAsync(sender, e).Wait();
        }


        public async Task AddChannel(string channelName)
        {
            AppDomain.CurrentDomain.ProcessExit += OnApplicationQuit;

            var mainVoice = GetWaitRoomName(channelName);
            var voice1Team = GetBlueTeamName(channelName);
            var voice2Team = GetRedTeamName(channelName);

            var createdChannelMainVoice = await LoLPlayManager.Instance.GetGuild().CreateVoiceChannelAsync(mainVoice, x => { x.UserLimit = GlobalConfig.TeamPlayWaitRoomMaxPlayer; x.CategoryId = GlobalConfig.TeamVsChannelID; });
            var createdChannelBlueTeam = await LoLPlayManager.Instance.GetGuild().CreateVoiceChannelAsync(voice1Team, x => { x.UserLimit = GlobalConfig.TeamPlayGameRoomMaxPlayer; x.CategoryId = GlobalConfig.TeamVsChannelID; });
            var createdChannelRedTeam = await LoLPlayManager.Instance.GetGuild().CreateVoiceChannelAsync(voice2Team, x => { x.UserLimit = GlobalConfig.TeamPlayGameRoomMaxPlayer; x.CategoryId = GlobalConfig.TeamVsChannelID; });

            //add
            createdTvTChannels.Add(createdChannelMainVoice);
            createdTvTChannels.Add(createdChannelBlueTeam);
            createdTvTChannels.Add(createdChannelRedTeam);
            

        }
        public async Task Initialize()
        {
            AppDomain.CurrentDomain.ProcessExit += OnApplicationQuit;
            foreach(var channel in TvTChannels) 
                await AddChannel(channel); 

        }

    }
}
