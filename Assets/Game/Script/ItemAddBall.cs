using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Script
{
    public class ItemAddBall : BaseBrick
    {
        public int sumBall = 1;

        public override void OnSpawn(int hp)
        {
            colBrick.isTrigger = true;
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            GameController.ins.OnAddBall(sumBall, transform.position);
            gameObject.SetActive(false);
        }
    }
}