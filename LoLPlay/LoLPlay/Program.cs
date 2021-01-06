using System;

namespace LoLPlay
{
    public class Program
    {
        public static void Main(string[] args)
        {
            while (true)
            {

            }
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
    }
}
