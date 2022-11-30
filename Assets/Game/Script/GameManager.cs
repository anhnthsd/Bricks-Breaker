using System;
using Game.Script.Data;
using Game.Script.Model;
using UnityEngine;

namespace Game.Script
{
    public class GameManager : MonoBehaviour
    {
        private static UserModel _userModel;
        private static MainMissionModel _mainMissionModel;
        private static DailyMissionModel _dailyMissionModel;
        private static LevelTowerModel _levelTowerModel;

        public static event Action UpdateDiamond;

        private void Awake()
        {
            UserModel.Ins.Init();
            MainMissionModel.Ins.Init();
            DailyMissionModel.Ins.Init();
            LevelTowerModel.Ins.Init();
        }

        private void Start()
        {
            _userModel = UserModel.Ins;
            _mainMissionModel = MainMissionModel.Ins;
            _dailyMissionModel = DailyMissionModel.Ins;
            _levelTowerModel = LevelTowerModel.Ins;
        }

        public static void Victory(int level, int star)
        {
            _levelTowerModel.Victory(level, star);
            _userModel.Victory(level);
        }

        public static void PlayClassic()
        {
            _mainMissionModel.ReportMission(TypeMissionMain.PlayClassic);
            _dailyMissionModel.ReportMission(TypeMissionDaily.PlayClassic);
        }

        public static void PlayTower()
        {
            _mainMissionModel.ReportMission(TypeMissionMain.PlayTower);
            _dailyMissionModel.ReportMission(TypeMissionDaily.PlayTower);
        }

        public static void GetStarTower(int star)
        {
            _mainMissionModel.ReportMission(TypeMissionMain.StarTower, star);
        }

        public static void ClaimQMain(TypeMissionMain type)
        {
            UserModel.Ins.ClaimDiamond(MainMissionModel.Ins.Claim(type));
            UpdateDiamond?.Invoke();
        }

        public static void ClaimQDaily(TypeMissionDaily type)
        {
            UserModel.Ins.ClaimDiamond(DailyMissionModel.Ins.Claim(type));
            UpdateDiamond?.Invoke();
        }
    }
}