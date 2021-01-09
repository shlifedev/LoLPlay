using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Text;

namespace LoLPlay.DBLog
{
    [FirestoreData]
    public class TierVeirfyRecord
    {
 

        [FirestoreProperty]
        public ulong DiscordID { get;set;}
        [FirestoreProperty]
        public string DiscordNick { get;set;}
        [FirestoreProperty]
        public string LOLNick { get; set; }
        [FirestoreProperty]
        public string SoloTier { get; set; } 

    }
}
