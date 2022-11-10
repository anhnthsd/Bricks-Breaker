using DG.Tweening;
using UnityEngine;

namespace Game.Script
{
    public class BallScript : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _rigi;
        public StateBall state = StateBall.Start;

        private void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.CompareTag("wallbottom"))
            {
                _rigi.velocity = Vector2.zero;
                transform.position = new Vector3(transform.position.x, -3.35f);
                state = StateBall.Done;
                GameController.ins.OnBallFall();
            }
        }

        public void Fly(Vector2 f)
        {
            if (state == StateBall.Start)
            {
                
                _rigi.AddForce(f);
                state = StateBall.Fly;
            }
        }

        public void ChangePosition(Vector2 newPos)
        {
            transform.DOMove(newPos, 0.2f);
        }
    }
}

public enum StateBall
{
    Start,Fly,Done
}