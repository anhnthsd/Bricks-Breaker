using System;
using UnityEngine;

namespace Game.Script
{
    public abstract class BaseModePlay : MonoBehaviour
    {
        private int _score;
        public int Score
        {
            get
            {
                return _score;
            }
            protected set
            {
                _score = value;
            }
        }
        public int currentRow;

        public int startBall;

        public BallController ballController;
        public BrickController brickController;
        public abstract void StartGame(int level);
        public abstract void AfterTurn();
        public abstract void SpecialTurn(int rows = 1);

        public abstract void EndGame();
        public abstract void EndMap();
        public abstract void IncreaseScore(Action<int,int> updateScore);
        public abstract void OnRestart();
        public abstract void Btn();

    }
}