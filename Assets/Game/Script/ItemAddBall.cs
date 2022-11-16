using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Script
{
    public class ItemAddBall : BaseBrick
    {
        public int sumBall = 1;
        public List<Sprite> lsBrickSprites;

        public override void OnSpawn(int hp)
        {
            colBrick.isTrigger = true;
            sumBall = hp;
            SetSprite(TypeOfBrick.AddBall);
        }

        public override void SetSprite(TypeOfBrick type)
        {
            switch (sumBall)
            {
                case 1:
                    srBrick.sprite = lsBrickSprites[1];
                    break;
                case 2:
                    srBrick.sprite = lsBrickSprites[2];
                    break;
                default:
                    sumBall = 3;
                    srBrick.sprite = lsBrickSprites[0];
                    break;
            }
        }

        public override void OnDelete()
        {
            gameObject.SetActive(false);
            BrickController.ins.DelBrick(i, j);
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
            BallController.ins.OnAddBall(count, pos);
            OnDelete();
        }
    }
}