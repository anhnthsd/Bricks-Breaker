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
            AddBall(sumBall, transform.position);
        }

        public void AddBall(int count, Vector3 pos)
        {
            GameController.ins.OnAddBall(count, pos);
            OnDelete();
            
        }
    }
}