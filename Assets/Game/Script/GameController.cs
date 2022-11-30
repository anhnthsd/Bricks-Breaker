using System;
using DG.Tweening;
using Game.Script.Data;
using Game.Script.Model;
using Game.Script.UI;
using TMPro;
using UnityEngine;
using Object = UnityEngine.Object;

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
        public Transform parentText;
        public GamePlayState gameState;
        public TextMeshProUGUI textPrefab;
        public event Action<GameMode> OnPlayGame;
        public Action<int, int> EventUpdateScore;
        public Action<bool> SetVisibleBtnBallReturn;

        public int levelPlay = 0;
        public GameMode currentGameMode;

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
            currentGameMode = gameMode;
            goPlay.SetActive(true);
            currentMode = modePlays[(int)gameMode];
            currentMode.gameObject.SetActive(true);
            currentMode.StartGame(level);
            OnPlayGame?.Invoke(gameMode);
            levelPlay = level;
        }

        public void Retry()
        {
            OnRestart();
            gameState = GamePlayState.Playing;
            goPlay.SetActive(true);
            currentMode = modePlays[(int)currentGameMode];
            currentMode.gameObject.SetActive(true);
            currentMode.StartGame(levelPlay);
            OnPlayGame?.Invoke(currentGameMode);
        }

        public void ActiveBallReturn(bool isActive)
        {
            SetVisibleBtnBallReturn?.Invoke(isActive);
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
            PopupManager.Show<UILose>(false);
        }

        public void EndMap()
        {
            currentMode.EndMap();
        }

        public void IncreaseScore()
        {
            currentMode.IncreaseScore(EventUpdateScore);
        }

        private int GetScore()
        {
            return currentMode.Score;
        }

        public void BtnStop()
        {
            if (gameState == GamePlayState.Stop)
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

        public void OnRestart()
        {
            currentMode.ballController.OnRestart();
            currentMode.brickController.OnRestart();
            currentMode.OnRestart();

            gameState = GamePlayState.Playing;
            EventUpdateScore?.Invoke(GetScore(), 0);
        }

        public void DelBrick(int i, int j)
        {
            currentMode.brickController.DelBrick(i, j);
        }

        public void OnDamage(Vector3 transformPosition, TypeOfBrick typeOfBrick, int i, int j)
        {
            currentMode.brickController.OnDamage(transformPosition, typeOfBrick, i, j);
        }

        public void OnBurst(Vector3 transformPosition, int i, int j, TypeOfBrick type)
        {
            currentMode.brickController.OnBurst(transformPosition, i, j, type);
        }

        public void BallReturn()
        {
            currentMode.ballController.BallReturn();
        }

        public void OnBallFall()
        {
            currentMode.ballController.OnBallFall();
        }

        public void OnAddBall(int count, Vector3 pos)
        {
            currentMode.ballController.OnAddBall(count, pos);
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