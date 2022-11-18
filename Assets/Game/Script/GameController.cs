using DG.Tweening;
using UnityEngine;

namespace Game.Script
{
    public class GameController : MonoBehaviour
    {
        public static GameController ins;
        public Camera cam;
        public int score = 0;
        
        private int brickOnTurn = 0;
        private int sumballSpecial = 9;
        private bool isSpecialTurn = false;

        private void Awake()
        {
            ins = this;
        }

        private void Start()
        {
            cam = Camera.main;
        }

        public void AfterTurn()
        {
            brickOnTurn = 0;
            if (isSpecialTurn)
            {
                BallController.ins.AfterSpecialTurn(sumballSpecial);
                isSpecialTurn = false;
            }
            BrickController.ins.AfterTurn();

            BallController.ins.AfterTurn();

            if (BrickController.ins.IsSpecialTurn())
            {
                Debug.Log("Special");
                isSpecialTurn = true;
                SpecialTurn(2);
            }
        }

        public void SpecialTurn(int rows = 1)
        {
            for (int r = 0; r < rows; r++)
            {
                DOVirtual.DelayedCall(0.2f, () => { BrickController.ins.AfterTurn(); });
            }

            BallController.ins.SpecialTurn(sumballSpecial);
        }

        public void EndGame()
        {
            Debug.Log("ENDGAME");
        }

        public void CreaseScore()
        {
            brickOnTurn++;
            score += 10 * brickOnTurn;
        }
    }
}