using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Script
{
    public class ItemDamage : BaseBrick
    {
        public TypeItem type = TypeItem.DamageHor;

        public override void OnSpawn(int hp)
        {
            colBrick.isTrigger = true;
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            GameController.ins.OnDamage(transform.position, type, j, i);
        }
    }
}