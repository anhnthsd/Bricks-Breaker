using System;
using System.Collections.Generic;
using DG.Tweening;
using Game.Script.Data;
using TMPro;
using UnityEngine;

namespace Game.Script
{
    public class NormalBrick : BaseBrick
    {
        public int hpBrick;
        public TextMeshProUGUI textBrick;
        public GameObject fxBrick;

        public override void OnSpawn(int hp)
        {
            hpBrick = hp;
            textBrick.text = hp.ToString();
            SetSprite(TypeOfBrick.Normal);
        }

        public override void SetSprite(TypeOfBrick type)
        {
            var lsBrickSprites = Resources.Load<DataBrick>("DataBrick").brickInfo.Find(s => s.type == type)
                .lsSprite;
            if (hpBrick < 2)
            {
                srBrick.sprite = lsBrickSprites[0];
            }
            else if (hpBrick < 4)
            {
                srBrick.sprite = lsBrickSprites[1];
            }
            else if (hpBrick <= 5)
            {
                srBrick.sprite = lsBrickSprites[2];
            }
            else if (hpBrick <= 10)
            {
                srBrick.sprite = lsBrickSprites[3];
            }
            else if (hpBrick <= 20)
            {
                srBrick.sprite = lsBrickSprites[4];
            }
            else if (hpBrick <= 30)
            {
                srBrick.sprite = lsBrickSprites[5];
            }
            else if (hpBrick <= 50)
            {
                srBrick.sprite = lsBrickSprites[6];
            }
            else if (hpBrick <= 70)
            {
                srBrick.sprite = lsBrickSprites[7];
            }
            else if (hpBrick <= 90)
            {
                srBrick.sprite = lsBrickSprites[8];
            }
            else if (hpBrick <= 120)
            {
                srBrick.sprite = lsBrickSprites[9];
            }
            else
            {
                srBrick.sprite = lsBrickSprites[10];
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

        private void Update()
        {
            UpdateTextPosition(transform.position);
        }

        public override void UpdateTextPosition(Vector2 pos)
        {
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
        }
    }
}