using System;
using UnityEngine;

namespace Game.Script
{
    public class BrickC : BaseBrick
    {
        public override void OnSpawn(int hp)
        {
            colBrick.isTrigger = true;
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            Debug.Log(col.gameObject.name);
        }
    }
}