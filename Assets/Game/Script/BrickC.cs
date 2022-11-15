using System;
using UnityEngine;

namespace Game.Script
{
    public class BrickC : BaseBrick
    {
        public TypeOfBrick type = TypeOfBrick.DamageHorizontal;

        public override void OnSpawn(int hp)
        {
            colBrick.isTrigger = true;
        }

        public override void OnDelete()
        {
            gameObject.SetActive(false);
        }

        public override void OnDamage()
        {
        }

        public override void SetPosition(Vector2 pos)
        {
            
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            Debug.Log(col.gameObject.name);
            if (type == TypeOfBrick.DamageHorizontal)
            {
                GameController.ins.OnAddBall(2, transform.position);
                OnDelete();
            }
            else
            {
                GameController.ins.OnDamage(transform.position, type, j, i);
                // GameController.ins.OnDamageBoth(j, i);
                // GameController.ins.OnDamageVer(i);
                // GameController.ins.OnDamageHor(j);
            }
        }
    }
}