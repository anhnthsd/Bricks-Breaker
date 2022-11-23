using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Script
{
    public class ItemDamage : BaseBrick
    {
        public GameObject fxDamageHor;
        public GameObject fxDamageVer;
        public bool isOver = false;
        public List<Sprite> lsBrickSprites;

        private void OnEnable()
        {
            BrickController.OnEndTurn += OnEndTurn;
        }

        private void OnDisable()
        {
            BrickController.OnEndTurn -= OnEndTurn;
        }

        public override void OnEndTurn()
        {
            if (isOver)
            {
                gameObject.SetActive(false);
                BrickController.ins.DelBrick(i, j);
            }
            else
            {
                //di xuong
            }
        }

        public override void OnSpawn(int hp)
        {
            colBrick.isTrigger = true;
        }

        public override void SetSprite(TypeOfBrick type)
        {
            this.typeOfBrick = type;
            switch (type)
            {
                case TypeOfBrick.DamageHorizontal:
                    srBrick.sprite = lsBrickSprites[1];
                    break;
                case TypeOfBrick.DamageVertical:
                    srBrick.sprite = lsBrickSprites[2];
                    break;
                default:
                    srBrick.sprite = lsBrickSprites[0];
                    break;
            }
        }

        public void TakeItemBurst()
        {
            gameObject.SetActive(false);
            BrickController.ins.DelBrick(i, j);
        }

        public override void SetPosition(Vector2 pos)
        {
            transform.position = pos;
        }

        public override void Active(bool isActive)
        {
            gameObject.SetActive(isActive);
        }

        public override void UpdatePosition(Vector2 pos)
        {
            transform.position = pos;
        }

        public override void TakeDamage()
        {
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            isOver = true;
            BrickController.ins.OnDamage(transform.position, typeOfBrick, j, i);
        }
    }
}