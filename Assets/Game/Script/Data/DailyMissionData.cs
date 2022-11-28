using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace Game.Script.Data
{
    [CreateAssetMenu(fileName = "DailyMission", menuName = "Data/DailyMission")]
    public class DailyMissionData : ScriptableObject
    {
        public List<MissionDaily> lsMissionDaily;

        [ContextMenu("Save")]
        public void Save()
        {
            var json = JsonConvert.SerializeObject(lsMissionDaily);
            PlayerPrefs.SetString("DailyMission", json);
        }

        [ContextMenu("Load")]
        public void Load()
        {
            var data = PlayerPrefs.GetString("DailyMission");
            lsMissionDaily = JsonConvert.DeserializeObject<List<MissionDaily>>(data);
        }
    }

    [Serializable]
    public class MissionDaily
    {
        public string name;
        public int requirement;
        public int amountDiamond;
        public TypeMissionDaily type;
    }

    public enum TypeMissionDaily
    {
        DoneMissionDaily,
        SpecialTurn,
        PlayClassic,
        PlayTower,
        SeeResult
    }
}