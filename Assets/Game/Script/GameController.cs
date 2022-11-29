using System;
using DG.Tweening;
using Game.Script.Data;
using Game.Script.Model;
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

        public GamePlayState gameState;

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
            gameState = GamePlayState.Playing;
            goPlay.SetActive(true);
            currentMode = modePlays[(int)gameMode];
            currentMode.gameObject.SetActive(true);
            currentMode.StartGame(level);
        }

        public void NextLevel(int level)
        {
            
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
            gameState = GamePlayState.Lose;
            PopupManager.Show<UIVictory>();
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

        public void BtnStop()
        {
            if (Time.timeScale == 0)
            {
                Time.timeScale = 1;
                gameState = GamePlayState.Playing;
            }
            else
            {
                Time.timeScale = 0;
                gameState = GamePlayState.Stop;
            }
        }
    }
}

public enum GamePlayState
{
    Start,
    Stop,
    Playing,
    Victory,
    Lose,
    Done
}