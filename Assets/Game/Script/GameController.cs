using DG.Tweening;
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
        public Text txtScore;
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

        public void SpecialTurn(int rows = 1)
        {
            currentMode.SpecialTurn(rows);
        }

        public void EndGame()
        {
            currentMode.EndGame();
            isEndGame = true;
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