using System;
using System.IO;
using DG.Tweening;
using Game.Script.UI;
using Newtonsoft.Json;
using UnityEngine;

namespace Game.Script.ModePlay
{
    public class ModeTower : BaseModePlay
    {
        [SerializeField] private int brickOnTurn = 0;
        [SerializeField] private bool isSpecialTurn = false;
        [SerializeField] private int sumBallSpecial = 9;

        public Action<int> eventUpdateScoreTower;
        private int levelCurrent;

        private int[,] _lsMap;
        private int[,] _lsNumber;

        public override void StartGame(int level)
        {
            ReadFile(level);
            levelCurrent = level;
            currentRow = 4;
            ballController.Play(startBall);
            brickController.CreateBrickWithMap(_lsMap, _lsNumber, currentRow);
            GameManager.PlayTower();
        }

        private void ReadFile(int level)
        {
            string path = "Assets/Resources/level" + level % 10 + ".txt";
            StreamReader reader = new StreamReader(path);
            var str = reader.ReadLine();
            if (str != null) _lsMap = JsonConvert.DeserializeObject<int[,]>(str);
            str = reader.ReadLine();
            if (str != null) _lsNumber = JsonConvert.DeserializeObject<int[,]>(str);
            reader.Close();
        }

        public override void AfterTurn()
        {
            brickOnTurn = 0;
            if (isSpecialTurn)
            {
                ballController.AfterSpecialTurn(sumBallSpecial);
                isSpecialTurn = false;
            }

            ballController.AfterTurn();

            brickController.AfterTurn();

            if (brickController.IsClearMap())
            {
                Debug.Log("Victory");
                GameController.ins.gameState = GamePlayState.Victory;
                PopupManager.Show<UIVictory>();
                GameManager.Victory(levelCurrent, 3);
            }

            if (brickController.IsSpecialTurn())
            {
                GameController.ins.SpecialTurn();
                isSpecialTurn = true;
                SpecialTurn(2);
            }
        }

        public override void SpecialTurn(int rows = 1)
        {
            for (int r = 0; r < rows; r++)
            {
                DOVirtual.DelayedCall(0.3f, () => { brickController.AfterTurn(); });
            }

            ballController.SpecialTurn(sumBallSpecial);
        }

        public override void EndGame()
        {
            Debug.Log("ENDGAME");
        }

        public override void EndMap()
        {
        }

        public override void IncreaseScore()
        {
            brickOnTurn++;
            Score += 10 * brickOnTurn;
            eventUpdateScoreTower?.Invoke(Score);
        }

        public override void Btn()
        {
            ballController.BallReturn();
        }
    }
}