using System;
using DG.Tweening;
using UnityEngine;

namespace Game.Script
{
    public class BallScript : MonoBehaviour
    {
        [SerializeField] public Rigidbody2D rigi;
        public StateBall state = StateBall.Start;

        private void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.CompareTag("wallbottom") && state != StateBall.Stop)
            {
                rigi.velocity = Vector2.zero;
                transform.position = new Vector3(transform.position.x, -3.33f);
                state = StateBall.Done;
                BallController.ins.OnBallFall();
            }
        }

        public void Fly(Vector2 f)
        {
            if (state == StateBall.Start)
            {
                rigi.AddForce(f);
                state = StateBall.Fly;
            }
        }

        public void ChangePosition(Vector2 newPos)
        {
            transform.DOMove(newPos, 0.2f).OnComplete((() =>
            {
                state = StateBall.Start;
            }));
        }
    }
}

public enum StateBall
{
    Start,
    Fly,
    Done,
    Stop
}