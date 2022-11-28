using System;
using DG.Tweening;
using Game.Script.UI;
using UnityEngine;

namespace Game.Script.ModePlay
{
    public class ModeTower : BaseModePlay
    {
        [SerializeField] private int brickOnTurn = 0;
        [SerializeField] private bool isSpecialTurn = false;
        [SerializeField] private int sumBallSpecial = 9;

        public Action<int> eventUpdateScoreTower;

        private readonly int[,] _lsMap = new int[,]
        {
            { 1, 1, 1, 1, 1, 1, 1 },
            { 0, 1, 1, 1, 1, 1, 1 },
            { 0, 2, 1, 1, 1, 1, 1 },
            { 0, 1, 1, 1, 1, 1, 1 },
            { 4, 1, 1, 1, 3, 1, 1 },
            { 0, 1, 1, 1, 1, 1, 1 },
            { 0, 1, 1, 1, 1, 1, 1 },
            { 1, 1, 1, 2, 1, 1, 1 },
            { 0, 1, 1, 1, 1, 1, 1 },
            { 0, 1, 1, 1, 1, 1, 1 },
            { 0, 1, 1, 1, 1, 3, 1 },
            { 0, 0, 3, 0, 1, 3, 1 },
            { 7, 8, 9, 10, 11, 1, 1 },
            { 0, 1, 2, 1, 1, 5, 1 },
        };
    
        private readonly int[,] _lsNumber = new int[,]
        {
            { 1, 1, 1, 1, 1, 1, 1 },
            { 1, 1, 1, 1, 2, 1, 1 },
            { 1, 2, 1, 3, 1, 1, 2 },
            { 1, 1, 1, 1, 2, 3, 1 },
            { 4, 1, 3, 1, 1, 1, 1 },
            { 0, 1, 1, 1, 1, 1, 1 },
            { 0, 1, 1, 1, 3, 1, 2 },
            { 1, 1, 1, 3, 1, 1, 1 },
            { 3, 1, 3, 1, 1, 1, 2 },
            { 1, 1, 1, 1, 1, 1, 1 },
            { 1, 3, 1, 3, 2, 1, 3 },
            { 1, 2, 1, 1, 1, 1, 1 },
            { 1, 1, 100, 100, 100, 100, 10 },
            { 100, 2, 1, 1, 1, 1, 3 },
        };
    
        public override void StartGame(int level)
        {
            currentRow = 4;
            ballController.Play(startBall);
            brickController.CreateBrickWithMap(_lsMap, _lsNumber, currentRow);
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
                PopupManager.Show<UIResult>();
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
