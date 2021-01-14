using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LoLPlay.Functions
{
    public class UserPunishment : Function<UserPunishment>
    {
        public readonly short MAX_WARNING = 3;
        /// <summary>
        /// 처벌
        /// </summary> 
        public async Task Punish(uint discordID)
        {

        }
        public async Task Forgive(uint discordID)
        {

        }
    }
}
