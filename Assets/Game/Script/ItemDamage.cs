using System;
using System.Collections.Generic;
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
        public List<Sprite> lsBrickSprites;

        public override void OnSpawn(int hp)
        {
            colBrick.isTrigger = true;
        }

        public override void SetSprite(TypeOfBrick type)
        {
            this.type = type;
            switch (type)
            {
                case TypeOfBrick.DamageHorizontal:
                    srBrick.sprite = lsBrickSprites[1];
                    break;
                case TypeOfBrick.DamageVertical:
                    srBrick.sprite = lsBrickSprites[2];
                    break;
                case TypeOfBrick.DamageBoth:
                    srBrick.sprite = lsBrickSprites[2];
                    break;
                default:
                    srBrick.sprite = lsBrickSprites[0];
                    break;
            }
        }

        public override void OnDelete()
        {
            gameObject.SetActive(false);
            BrickController.ins.DelBrick(i, j);
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
            BrickController.ins.OnDamage(transform.position, type, j, i);
        }
    }
}