using System;
using System.Collections;
using System.Collections.Generic;
using Game.Script.Data;
using Newtonsoft.Json;
using UnityEngine;

public class MainMissionModel
{
    private static MainMissionModel _ins;

    public static MainMissionModel Ins
    {
        get
        {
            if (_ins == null)
            {
                _ins = new MainMissionModel();
            }

            return _ins;
        }
    }

    public List<MainMissionInfo> missionInfos;

    public void Init()
    {
        var data = Resources.Load<MainMissionData>("MainMission");
        missionInfos = new List<MainMissionInfo>();
        for (int i = 0; i < data.lsMissionMain.Count; i++)
        {
            missionInfos.Add(MainMissionInfo.InitNew(data.lsMissionMain[i]));
        }
    }

    public void ReportMission(TypeMissionMain typeMissionMain, int amount = 1)
    {
        for (int i = 0; i < missionInfos.Count; i++)
        {
            if (typeMissionMain.Equals(missionInfos[i].type))
            {
                missionInfos[i].amountProgress += amount;
                if (missionInfos[i].amountProgress >= missionInfos[i].requirement)
                {
                    missionInfos[i].isComplete = true;
                    missionInfos[i].canClaim = true;
                }
            }
        }

        Save();
    }

    public void Claim(TypeMissionMain typeMissionMain)
    {
        for (int i = 0; i < missionInfos.Count; i++)
        {
            if (typeMissionMain.Equals(missionInfos[i].type))
            {
                if (missionInfos[i].canClaim)
                {
                    missionInfos[i].canClaim = false;
                    missionInfos[i].isComplete = false;
                    missionInfos[i].requirement += missionInfos[i].increaseRequirement;
                }
            }
        }

        Save();
    }

    public void Save()
    {
        var json = JsonConvert.SerializeObject(missionInfos);
        PlayerPrefs.SetString("mainMission", json);
    }

    public void Load()
    {
        var dataStr = PlayerPrefs.GetString("mainMission");
        missionInfos = JsonConvert.DeserializeObject<List<MainMissionInfo>>(dataStr);
    }
}

[Serializable]
public class MainMissionInfo
{
    public string name;
    public int requirement;
    public int increaseRequirement;
    public int amountProgress;
    public int amountDiamond;
    public bool isComplete;
    public bool canClaim;
    public TypeMissionMain type;

    public static MainMissionInfo InitNew(MissionMain missionMain)
    {
        var m = new MainMissionInfo();
        m.name = missionMain.name;
        m.requirement = missionMain.requirement;
        m.increaseRequirement = missionMain.increaseRequirement;
        m.amountProgress = 0;
        m.amountDiamond = missionMain.amountDiamond;
        m.isComplete = false;
        m.canClaim = false;
        m.type = missionMain.type;
        return m;
    }
}