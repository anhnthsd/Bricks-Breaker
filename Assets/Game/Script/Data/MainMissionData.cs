using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace Game.Script.Data
{
    [CreateAssetMenu(fileName = "MainMission", menuName = "Data/MainMission")]
    public class MainMissionData : ScriptableObject
    {
        public List<MissionMain> lsMissionMain;

        [ContextMenu("Save")]
        public void Save()
        {
            var js = JsonConvert.SerializeObject(lsMissionMain);

            PlayerPrefs.SetString("MainMission", js);
        }

        [ContextMenu("Load")]
        public void Load()
        {
            var s = PlayerPrefs.GetString("MainMission");

            lsMissionMain = JsonConvert.DeserializeObject<List<MissionMain>>(s);
        }
    }

    [Serializable]
    public class MissionMain
    {
        public string name;
        public int requirement;
        public int increaseRequirement;
        public int amountDiamond;
        public TypeMissionMain type;
    }

    public enum TypeMissionMain
    {
        PlayTower,
        PlayClassic,
        StarTower,
        Continue,
        UseItem
    }
}