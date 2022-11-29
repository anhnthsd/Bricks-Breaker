using System;
using Game.Script.Data;
using Game.Script.Model;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Script.ModePlay
{
    public class ModeClassic : BaseModePlay
    {
        // [SerializeField] private int brickOnTurn = 0;
        [SerializeField] private bool isSpecialTurn = false;
        [SerializeField] private int sumBallSpecial = 9;

        public Action<int> eventUpdateScoreClassic;
        private int lvl = 1;

        private int[,] _lsMap = new int[,]
        {
            { 1, 1, 1, 1, 1, 1, 7 },
            { 0, 1, 7, 1, 1, 1, 1 },
            { 0, 1, 1, 1, 7, 1, 1 },
            { 0, 1, 1, 1, 1, 0, 7 },
            { 1, 1, 1, 7, 1, 1, 7 },
            { 0, 1, 1, 0, 1, 1, 1 },
            { 0, 1, 7, 1, 1, 0, 1 },
            { 1, 7, 1, 1, 1, 1, 7 },
            { 0, 1, 1, 1, 1, 1, 1 },
            { 0, 1, 1, 1, 7, 1, 1 },
        };

        private int[,] _lsNumber = new int[,]
        {
            { 8, 1, 6, 1, 1, 10, 1 },
            { 0, 10, 1, 1, 2, 8, 1 },
            { 9, 2, 1, 3, 1,9, 2 },
            { 0, 1, 7, 1, 2, 3, 3 },
            { 4, 6, 3, 1, 1, 1, 1 },
            { 0, 5, 1, 1, 1, 5, 1 },
            { 0, 1, 1, 1, 3, 4, 2 },
            { 1, 1, 2, 1, 1, 1, 1 },
            { 3, 1, 3, 1, 1, 1, 2 },
            { 1, 1, 1, 1, 1, 1, 1 },
        };

        public override void StartGame(int level)
        {
            currentRow = 1;
            ballController.Play(startBall);
            brickController.CreateBrickWithMap(_lsMap, _lsNumber, currentRow);
            GameManager.PlayClassic();
        }

        public override void AfterTurn()
        {
            if (isSpecialTurn)
            {
                ballController.AfterSpecialTurn(sumBallSpecial);
                isSpecialTurn = false;
            }

            ballController.AfterTurn();

            brickController.AfterTurn();
            IncreaseScore();
        }

        public override void SpecialTurn(int rows = 1)
        {
        }

        public override void EndGame()
        {
            Debug.Log("ENDGAME");
        }

        public override void EndMap()
        {
            lvl++;
            CreateMap();
            brickController.AddNewMap(_lsMap,_lsNumber);
        }

        public override void IncreaseScore()
        {
            Score++;
            eventUpdateScoreClassic?.Invoke(Score);
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