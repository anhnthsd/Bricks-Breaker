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
            CheckTime();
        }

        [ContextMenu("Save")]
        private void SaveTime()
        {
            var t = MyUtils.DateTimeToTimeStamp(DateTime.Now);
            PlayerPrefs.SetString("timeLogin", t.ToString());
        }

        [ContextMenu("GetTime")]
        private void GetTime()
        {
            var t = long.Parse(PlayerPrefs.GetString("timeLogin"));
            Debug.Log(MyUtils.UnixTimeStampToDateTime(t));
        }

        [ContextMenu("Check")]
        private void CheckTime()
        {
            if (PlayerPrefs.HasKey("timeLogin"))
            {
                var t = long.Parse(PlayerPrefs.GetString("timeLogin"));
                var timeLast = MyUtils.UnixTimeStampToDateTime(t);

                var timeNow = DateTime.Now;
                Debug.Log(timeNow.Day - timeLast.Day);

                if (timeNow.Day - timeLast.Day > 0)
                {
                    _dailyMissionModel.ResetMission();
                }
            }

            SaveTime();
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
            _userModel.ClaimDiamond(MainMissionModel.Ins.Claim(type));
            UpdateDiamond?.Invoke();
        }

        public static void ClaimQDaily(TypeMissionDaily type)
        {
            _userModel.ClaimDiamond(DailyMissionModel.Ins.Claim(type));
            UpdateDiamond?.Invoke();
        }

        public static void SaveScoreClassic(int score)
        {
            _userModel.NewBestScore(score);
        }
    }
}