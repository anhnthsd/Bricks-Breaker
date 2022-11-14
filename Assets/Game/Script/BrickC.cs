using System;
using UnityEngine;

namespace Game.Script
{
    public class BrickC : BaseBrick
    {
        public TypeItem type = TypeItem.AddBall;

        public override void OnSpawn(int hp)
        {
            colBrick.isTrigger = true;
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            Debug.Log(col.gameObject.name);
            if (type == TypeItem.AddBall)
            {
                GameController.ins.OnAddBall(2, transform.position);
                gameObject.SetActive(false);
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

public enum TypeItem
{
    AddBall,
    DamageHor,
    DamageVer,
    DamageBoth
}