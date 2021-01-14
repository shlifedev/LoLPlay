using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Text;

namespace LoLPlay.DBLog
{
    [FirestoreData]
    public class UserPunishmentRecord
    { 
        [FirestoreProperty]
        public string DateTime { get; set; }
        [FirestoreProperty]
        public ulong JudgeID { get; set; }
        [FirestoreProperty]
        public string JudgeNickname { get; set; }
        [FirestoreProperty]
        public ulong PunishTargetID { get; set; }
        [FirestoreProperty]
        public string PunishTargetNickName { get; set; }
        /// <summary>
        /// 처벌 사유
        /// </summary>
        [FirestoreProperty]
        public string Reason { get; set; } 
        /// <summary>
        /// 값이 true인경우 처벌이 유효함.
        /// false인경우 처벌 카운트에 집계하지 않음.
        /// </summary>
        [FirestoreProperty]
        public bool Valid { get; set; }

    }
}
