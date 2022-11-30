using Game.Script.Data;
using UnityEngine;

namespace Game.Script
{
    public class ItemDamage : BaseBrick
    {
        public bool isOver = false;

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
                GameController.ins.DelBrick(i, j);
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
            var lsBrickSprites = Resources.Load<DataBrick>("DataBrick").brickInfo.Find(s => s.type == type)
                .lsSprite;
            typeOfBrick = type;
            switch (type)
            {
                case TypeOfBrick.DamageHorizontal:
                    srBrick.sprite = lsBrickSprites[0];
                    break;
                case TypeOfBrick.DamageVertical:
                    srBrick.sprite = lsBrickSprites[1];
                    break;
                default:
                    srBrick.sprite = lsBrickSprites[2];
                    break;
            }
        }

        public override void SetPosition(Vector2 pos)
        {
            transform.position = pos;
        }

        public override void Active(bool isActive)
        {
            gameObject.SetActive(isActive);
        }

        public override void TakeDamage()
        {
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            isOver = true;
            GameController.ins.OnDamage(transform.position, typeOfBrick, i, j);
        }
    }
}