using System;
using Game.Script.Mission;
using UnityEngine;

namespace Game.Script
{
    public class GameManager : MonoBehaviour
    {
        private void Awake()
        {
            MainMissionModel.Ins.Init();
            DailyMissionModel.Ins.Init();
        }
    }
}
