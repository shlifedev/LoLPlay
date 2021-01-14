using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
namespace LoLPlay
{
    public static class DB
    {
        public static bool dbInitialized = false;
        static FirestoreDb db = null;
        /// <summary>
        /// 티어 인증 기록 남기기
        /// </summary>
        /// <param name="discordID"></param>
        /// <param name="discordName"></param>
        /// <param name="lolNick"></param>
        /// <param name="tier"></param>
        public static async Task RecordTierVerify(ulong discordID, string discordName, string lolNick, string tier)
        {
            if(dbInitialized == false) return;

            await db.Collection("tierVerify").Document(discordID.ToString()).Collection("records").AddAsync(new DBLog.TierVeirfyRecord()
            {
                DiscordID = discordID,
                DiscordNick = discordName,
                LOLNick = lolNick,
                SoloTier = tier,
                DateTime = System.DateTime.Now.ToString()
            });;
        }


        public static async Task<List<DBLog.TierVeirfyRecord>> GetTierVerifyRecordAsync(ulong id)
        {
            var cols = db.Collection("tierVerify").Document(id.ToString()).Collection("records");
            var snap = await cols.GetSnapshotAsync();
            List<DBLog.TierVeirfyRecord> records = new List<DBLog.TierVeirfyRecord>();
            foreach (var value in snap.Documents)
            {
                var data = value.ConvertTo<DBLog.TierVeirfyRecord>();
                records.Add(data);
            } 
            return records;
        }

        public static async Task DBInit()
        {
            FirestoreDbBuilder builder = new FirestoreDbBuilder(){ CredentialsPath ="LoLPlay-6e4ac4a73f47.json", ProjectId = "lolplay-fba13"};
            db = builder.Build();
            dbInitialized = true; 
            var cols = db.Collection("tierVerify").Document("229808086091563010").Collection("records");
            var snap = await cols.GetSnapshotAsync(); 
            foreach (var value in snap.Documents)
            {
                var data = value.ConvertTo<DBLog.TierVeirfyRecord>();
                Console.WriteLine(data.LOLNick);
            }
          

        }
    }
}
