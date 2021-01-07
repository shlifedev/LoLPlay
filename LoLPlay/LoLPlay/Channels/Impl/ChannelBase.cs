using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LoLPlay.Channels
{
    public class ChannelBase 
    { 
        public SocketChannel SocketChannel => LoLPlayManager.Instance.Client.GetChannel(ID);
        public System.Collections.Generic.HashSet<string> commands = new HashSet<string>();
        public ulong ID;


        public virtual async Task OnReceivedMsg(SocketUserMessage message, string command, List<string> args)
        {

        }

        public virtual async Task OnApplicationQuitAsync(object sender, EventArgs e)
        {
             
        }
        public virtual void OnApplicationQuit(object sender, EventArgs e)
        {
           
        }

        public bool CommandExist(string command)
        {
            return commands.Contains(command);
        }
        public void AddCommand(string command)
        {
            commands.Add(command);
            Console.ForegroundColor = ConsoleColor.Green;
            LoLPlayManager.Instance.Log(new Discord.LogMessage(Discord.LogSeverity.Debug, "ChannelInstance", $"AddCommand - {command}"));
            Console.ForegroundColor = ConsoleColor.White;
        }
        public void Init(ulong channelId)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            LoLPlayManager.Instance.Log(new Discord.LogMessage(Discord.LogSeverity.Debug, "ChannelInstance", $"Initalized Channel Instance - {this.GetType().Name}"));
            Console.ForegroundColor = ConsoleColor.White;
            this.ID = channelId;
        }  
    }
}
