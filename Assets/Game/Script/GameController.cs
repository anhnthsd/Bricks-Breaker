using DG.Tweening;
using UnityEngine;

namespace Game.Script
{
    public class GameController : MonoBehaviour
    {
        public static GameController ins;
        public Camera cam;
        public BaseModePlay currentMode;

        private void Awake()
        {
            ins = this;
        }

        private void Start()
        {
            cam = Camera.main;
            PlayGame();
        }

        public void PlayGame()
        {
            currentMode.StartGame();
        }

        public void AfterTurn()
        {
            currentMode.AfterTurn();
        }

        public void SpecialTurn(int rows = 1)
        {
            currentMode.SpecialTurn(rows);
        }

        public void EndGame()
        {
            currentMode.EndGame();
        }

        public void IncreaseScore()
        {
            currentMode.IncreaseScore();
        }

        public int GetScore()
        {
            return currentMode.Score;
        }
    }
}