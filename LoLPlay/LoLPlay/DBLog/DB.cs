using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using LoLPlay.DBLog;

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
            if (dbInitialized == false) return;

            await db.Collection("tierVerify").Document(discordID.ToString()).Collection("records").AddAsync(new DBLog.TierVeirfyRecord()
            {
                DiscordID = discordID,
                DiscordNick = discordName,
                LOLNick = lolNick,
                SoloTier = tier,
                DateTime = System.DateTime.Now.ToString()
            }); ;
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



        public static async Task RecordPunishment(uint judgeID, uint punishmentTargetID, string reason)
        {
            var judgeUser = LoLPlayManager.Instance.Client.GetUser(judgeID);
            var punishmentTargetUser = LoLPlayManager.Instance.Client.GetUser(punishmentTargetID);

            if (judgeUser != null && punishmentTargetUser != null)
            {
                UserPunishmentRecord record = new UserPunishmentRecord();
                record.DateTime = System.DateTime.Now.ToString();
                record.JudgeID = judgeID;
                record.JudgeNickname = judgeUser.Username;
                record.PunishTargetID = punishmentTargetID;
                record.PunishTargetNickName = punishmentTargetUser.Username;
                record.Reason = reason;
                record.Valid = true;
                await db.Collection("punishments").Document(punishmentTargetID.ToString()).Collection("records").AddAsync(record);
            }
        }

        /// <summary>
        /// 처벌 기록 가져오기
        /// </summary>
        /// <param name="discordID"></param>
        /// <returns></returns>
        public static async Task<List<DBLog.UserPunishmentRecord>> GetPunishmentRecordAsync(ulong discordID)
        {
            var cols = db.Collection("records").Document(discordID.ToString()).Collection("records");
            var snapshot = await cols.GetSnapshotAsync();
            List<DBLog.UserPunishmentRecord> records = new List<DBLog.UserPunishmentRecord>();
            foreach (var value in snapshot.Documents)
            { 
                var data = value.ConvertTo<DBLog.UserPunishmentRecord>();
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
