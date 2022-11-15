using System;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Game.Script
{
    public class ItemShootBall : BaseBrick
    {
        public bool isOver = false;

        public override void OnSpawn(int hp)
        {
            colBrick.isTrigger = true;
        }

        public override void OnDelete()
        {
            gameObject.SetActive(false);
            GameController.ins.DelBrick(i, j);
        }

        public override void OnDamage()
        {
        }

        public override void SetPosition(Vector2 pos)
        {
            transform.position = pos;
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            isOver = true;
            var ballS = col.GetComponent<BallScript>();
            if (ballS.state == StateBall.Fly)
            {
                ballS._rigi.velocity = Vector2.zero;
                Vector2 f = new Vector2(Random.Range(0.5f, 1), Random.Range(0.5f, 1)) * 450;
                ballS._rigi.AddForce(f);
            }
        }
    }
}