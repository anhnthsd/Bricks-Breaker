using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using DG.Tweening;
using Game.Script.Model;
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

        private int _levelCurrent = 1;
        private int _star = 0;

        private int[,] _lsMap;
        private int[,] _lsNumber;

        public override void StartGame(int level)
        {
            ReadFile(level);
            _levelCurrent = level;
            currentRow = 4;
            ballController.Play(startBall);
            brickController.CreateBrickWithMap(_lsMap, _lsNumber, currentRow);
            GameManager.PlayTower();
        }

        public override void Retry()
        {
            currentRow = 4;
            ballController.Play(startBall);
            brickController.CreateBrickWithMap(_lsMap, _lsNumber, currentRow);
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
                GameController.ins.gameState = GamePlayState.Victory;
                PopupManager.Show<UIVictory>(false).UpdateView(_star);
                GameManager.Victory(_levelCurrent, _star);
                // GameController.ins.EventUpdateScore?.Invoke(Score, _star);
            }

            if (brickController.IsSpecialTurn())
            {
                GameController.ins.SpecialTurn();
                isSpecialTurn = true;
                SpecialTurn(3);
            }
        }

        public override void SpecialTurn(int rows = 1)
        {
            for (int r = 0; r < rows; r++)
            {
                StartCoroutine(CreateBrickSpecialTurn(0.3f * (r + 1)));
            }

            ballController.SpecialTurn(sumBallSpecial);

            // GameController.ins.gameState = GamePlayState.Playing;
        }

        IEnumerator CreateBrickSpecialTurn(float time)
        {
            yield return new WaitForSeconds(time);
            brickController.AfterTurn();
        }

        public override void EndGame()
        {
            PopupManager.Show<UILose>(false).SetTowerView(_levelCurrent);
        }

        public override void EndMap()
        {
        }

        public override void IncreaseScore(Action<int, int> updateScore)
        {
            brickOnTurn++;
            Score += 10 * brickOnTurn;

            var maxScore = LevelTowerModel.Ins.levelInfos[_levelCurrent].scoreStar;
            if (Score > maxScore)
            {
                _star = 3;
            }
            else if (Score > maxScore * 0.7)
            {
                _star = 2;
            }
            else if (Score > 10)
            {
                _star = 1;
            }

            updateScore?.Invoke(Score, _star);
        }

        public override void OnRestart()
        {
            Score = 0;
            ballController.OnRestart();
            brickController.OnRestart();
        }

        public override void Btn()
        {
            ballController.BallReturn();
        }
    }
}