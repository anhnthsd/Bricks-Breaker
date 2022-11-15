﻿using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Script
{
    public class ItemDamage : BaseBrick
    {
        public TypeOfBrick type = TypeOfBrick.DamageHorizontal;
        public GameObject fxDamageHor;
        public GameObject fxDamageVer;
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

        public override void SetPosition(Vector2 pos)
        {
            transform.position = pos;
        }

        public override void OnDamage()
        {
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            isOver = true;
            GameController.ins.OnDamage(transform.position, type, j, i);
        }
    }
}