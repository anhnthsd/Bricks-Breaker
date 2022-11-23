using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Game.Script
{
    public class BurstBrick : BaseBrick
    {
        public TypeOfBrick type = TypeOfBrick.DeleteHorizontal;
        public int hpBrick;
        public TextMeshProUGUI textBrick;
        public GameObject fxBrick;
        public List<Sprite> lsBrickSprites;

        public override void OnSpawn(int hp)
        {
            hpBrick = hp;
            textBrick.text = hp.ToString();
        }

        public override void SetSprite(TypeOfBrick type)
        {
            this.type = type;
            switch (type)
            {
                case TypeOfBrick.DeleteHorizontal:
                    srBrick.sprite = lsBrickSprites[1];
                    break;
                case TypeOfBrick.DeleteVertical:
                    srBrick.sprite = lsBrickSprites[2];
                    break;
                case TypeOfBrick.DeleteBoth:
                    srBrick.sprite = lsBrickSprites[3];
                    break;
                default:
                    srBrick.sprite = lsBrickSprites[0];
                    break;
            }
        }

        public void TakeItemBurst()
        {
            DestroyBrick();
        }

        public override void SetPosition(Vector2 pos)
        {
            transform.position = pos;
            textBrick = Instantiate(BrickController.ins.textPrefab);
            textBrick.GetComponent<RectTransform>().anchoredPosition = GameController.ins.cam.WorldToScreenPoint(pos);
            textBrick.transform.SetParent(BrickController.ins.parentText);
        }

        public override void Active(bool isActive)
        {
            textBrick.gameObject.SetActive(isActive);
            gameObject.SetActive(isActive);
        }

        public override void UpdatePosition(Vector2 pos)
        {
            transform.position = pos;
            textBrick.transform.SetParent(null);
            textBrick.GetComponent<RectTransform>().anchoredPosition = GameController.ins.cam.WorldToScreenPoint(pos);
            textBrick.transform.SetParent(BrickController.ins.parentText);
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.CompareTag("ball"))
            {
                var fx = Instantiate(fxBrick);
                fx.transform.position = transform.position;
                TakeDamage();
                Destroy(fx, 0.3f);
            }
        }

        public override void TakeDamage()
        {
            hpBrick--;
            textBrick.text = hpBrick.ToString();
            if (hpBrick == 0)
            {
                DestroyBrick();
            }
        }

        public override void DestroyBrick()
        {
            base.DestroyBrick();
            textBrick.gameObject.SetActive(false);
            BrickController.ins.OnBurst(transform.position, j, i, type);
        }
    }
}