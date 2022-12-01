using System;
using System.IO;
using Game.Script.Data;
using Game.Script.Model;
using Game.Script.UI;
using Newtonsoft.Json;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Script.ModePlay
{
    public class ModeClassic : BaseModePlay
    {
        // [SerializeField] private int brickOnTurn = 0;
        [SerializeField] private bool isSpecialTurn = false;
        [SerializeField] private int sumBallSpecial = 9;

        private int lvl = 1;

        private int[,] _lsMap;

        private int[,] _lsNumber;

        public override void StartGame(int level)
        {
            ReadFile(Random.Range(0, 5));
            GameController.ins.UpdateBestScore?.Invoke();
            currentRow = 1;
            ballController.Play(startBall);
            brickController.CreateBrickWithMap(_lsMap, _lsNumber, currentRow);
            GameManager.PlayClassic();
        }

        public override void Retry()
        {
            ReadFile(Random.Range(0, 5));
            GameController.ins.UpdateBestScore?.Invoke();
            currentRow = 1;
            ballController.Play(startBall);
            brickController.CreateBrickWithMap(_lsMap, _lsNumber, currentRow);
        }

        private void ReadFile(int level)
        {
            string path = "Assets/Resources/classic_level" + level + ".txt";
            StreamReader reader = new StreamReader(path);
            var str = reader.ReadLine();
            if (str != null) _lsMap = JsonConvert.DeserializeObject<int[,]>(str);
            str = reader.ReadLine();
            if (str != null) _lsNumber = JsonConvert.DeserializeObject<int[,]>(str);
            reader.Close();
        }

        public override void AfterTurn()
        {
            Score++;

            if (isSpecialTurn)
            {
                ballController.AfterSpecialTurn(sumBallSpecial);
                isSpecialTurn = false;
            }

            ballController.AfterTurn();

            brickController.AfterTurn();

            GameController.ins.IncreaseScore();
        }

        public override void SpecialTurn(int rows = 1)
        {
        }

        public override void EndGame()
        {
            PopupManager.Show<UILose>(false).SetClassicView(Score);
            GameManager.SaveScoreClassic(Score);
        }

        public override void EndMap()
        {
            lvl++;
            CreateMap();
            brickController.AddNewMap(_lsMap, _lsNumber);
        }

        public override void IncreaseScore(Action<int, int> updateScore)
        {
            updateScore?.Invoke(Score, 0);
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

        private void CreateMap()
        {
            for (int i = 0; i < _lsMap.GetLength(0); i++)
            {
                int typeBrick = 2;
                for (int j = 0; j < _lsMap.GetLength(1); j++)
                {
                    _lsMap[i, j] = typeBrick <= 2 ? Random.Range(0, 11) : Random.Range(0, 2);
                    if (_lsMap[i, j] > 2 || _lsMap[i, j] == 0)
                    {
                        typeBrick++;
                    }

                    _lsNumber[i, j] = Random.Range(1, 10 * lvl);
                }
            }
        }
    }
}