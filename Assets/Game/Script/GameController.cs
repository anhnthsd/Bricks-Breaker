using DG.Tweening;
using UnityEngine;

namespace Game.Script
{
    public class GameController : MonoBehaviour
    {
        public static GameController ins;
        public Camera cam;

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
            BrickController.ins.AfterTurn();

            BallController.ins.AfterTurn();

            if (BrickController.ins.IsSpecialTurn())
            {
                Debug.Log("Special");
                SpecialTurn(2);
            }
        }

        public void SpecialTurn(int rows = 1)
        {
            for (int r = 0; r < rows; r++)
            {
                DOVirtual.DelayedCall(0.2f, () => { BrickController.ins.AfterTurn();});
            }
            BallController.ins.SpecialTurn(99);
        }
    }
}