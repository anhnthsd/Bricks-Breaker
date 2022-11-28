using DG.Tweening;
using Game.Script.Data;
using Game.Script.Mission;
using Game.Script.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Script
{
    public class GameController : MonoBehaviour
    {
        public static GameController ins;
        public Camera cam;
        public BaseModePlay currentMode;

        [SerializeField] private BaseModePlay[] modePlays;
        public bool isEndGame = false;
        public GameObject goPlay;
        
        private void Awake()
        {
            ins = this;
        }

        private void Start()
        {
            cam = Camera.main;
        }

        public void PlayGame(GameMode gameMode, int level)
        {
            goPlay.SetActive(true);
            currentMode = modePlays[(int)gameMode];
            currentMode.gameObject.SetActive(true);
            currentMode.StartGame(level);
        }

        public void AfterTurn()
        {
            if (!isEndGame)
            {
                currentMode.AfterTurn();
            }
        }

        public void SpecialTurn()
        {
            DailyMissionModel.Ins.ReportMission(TypeMissionDaily.SpecialTurn);
        }

        public void EndGame()
        {
            currentMode.EndGame();
            isEndGame = true;
            PopupManager.Show<UIResult>();
        }

        public void EndMap()
        {
            currentMode.EndMap();
        }

        public void IncreaseScore()
        {
            currentMode.IncreaseScore();
        }

        public int GetScore()
        {
            return currentMode.Score;
        }

        public void Btn()
        {
            currentMode.Btn();
        }
    }
}

public enum GamePlayState
{
    Start,
    Playing,
    Victory,
    Lose
}