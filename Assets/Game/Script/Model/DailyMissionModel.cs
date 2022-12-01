using System;
using System.Collections.Generic;
using Game.Script.Data;
using Newtonsoft.Json;
using UnityEngine;

namespace Game.Script.Model
{
    public class DailyMissionModel
    {
        public static DailyMissionModel _ins;

        public List<DailyMissionInfo> missionInfos;

        public static DailyMissionModel Ins
        {
            get
            {
                if (_ins == null)
                {
                    _ins = new DailyMissionModel();
                }

                return _ins;
            }
        }

        public void Init()
        {
            if (PlayerPrefs.HasKey("DailyMission"))
            {
                Load();
            }
            else
            {
                var data = Resources.Load<DailyMissionData>("DailyMission");
                missionInfos = new List<DailyMissionInfo>();
                for (int i = 0; i < data.lsMissionDaily.Count; i++)
                {
                    missionInfos.Add(DailyMissionInfo.InitNew(data.lsMissionDaily[i]));
                }

                Save();
            }

            // Load();
            // for (int i = 0; i < missionInfos.Count; i++)
            // {
            //     if (missionInfos[i].isComplete)
            //     {
            //         missionInfos[i].canClaim = true;
            //     }
            // }
            //
            // Save();
        }

        public void ResetMission()
        {
            var data = Resources.Load<DailyMissionData>("DailyMission");
            missionInfos = new List<DailyMissionInfo>();
            for (int i = 0; i < data.lsMissionDaily.Count; i++)
            {
                missionInfos.Add(DailyMissionInfo.InitNew(data.lsMissionDaily[i]));
            }

            Save();
        }

        public void ReportMission(TypeMissionDaily typeMissionDaily, int progress = 1)
        {
            for (int i = 0; i < missionInfos.Count; i++)
            {
                if (typeMissionDaily.Equals(missionInfos[i].type))
                {
                    missionInfos[i].amountProgress += progress;

                    if (missionInfos[i].amountProgress >= missionInfos[i].requirement)
                    {
                        missionInfos[i].isComplete = true;
                        missionInfos[i].canClaim = true;
                        if (typeMissionDaily != TypeMissionDaily.DoneMissionDaily)
                        {
                            ReportMission(TypeMissionDaily.DoneMissionDaily);
                        }
                    }
                }
            }

            Save();
        }

        public int Claim(TypeMissionDaily typeMissionDaily)
        {
            var diamond = 0;
            for (int i = 0; i < missionInfos.Count; i++)
            {
                if (typeMissionDaily.Equals(missionInfos[i].type))
                {
                    missionInfos[i].canClaim = false;
                    diamond = missionInfos[i].amountDiamond;
                }
            }

            Save();
            return diamond;
        }

        public void Save()
        {
            var json = JsonConvert.SerializeObject(missionInfos);
            PlayerPrefs.SetString("DailyMission", json);
            PlayerPrefs.Save();
        }

        public void Load()
        {
            var data = PlayerPrefs.GetString("DailyMission");
            missionInfos = JsonConvert.DeserializeObject<List<DailyMissionInfo>>(data);
        }
    }
}

[Serializable]
public class DailyMissionInfo
{
    public string name;
    public int requirement;
    public int amountProgress;
    public int amountDiamond;
    public bool isComplete;
    public bool canClaim;
    public TypeMissionDaily type;

    public static DailyMissionInfo InitNew(MissionDaily missionDaily)
    {
        var m = new DailyMissionInfo();
        m.name = missionDaily.name;
        m.requirement = missionDaily.requirement;
        m.amountProgress = 0;
        m.amountDiamond = missionDaily.amountDiamond;
        m.isComplete = false;
        m.canClaim = false;
        m.type = missionDaily.type;
        return m;
    }
}