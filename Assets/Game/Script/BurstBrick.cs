using System;
using System.Collections.Generic;
using DG.Tweening;
using Game.Script.Data;
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

        public override void OnSpawn(int hp)
        {
            hpBrick = hp;
            textBrick.text = hp.ToString();
        }

        public override void SetSprite(TypeOfBrick type)
        {
            var lsBrickSprites = Resources.Load<DataBrick>("DataBrick").brickInfo.Find(s => s.type == type)
                .lsSprite;
            this.type = type;
            switch (type)
            {
                case TypeOfBrick.DeleteHorizontal:
                    srBrick.sprite = lsBrickSprites[0];
                    break;
                case TypeOfBrick.DeleteVertical:
                    srBrick.sprite = lsBrickSprites[1];
                    break;
                case TypeOfBrick.DeleteBoth:
                    srBrick.sprite = lsBrickSprites[2];
                    break;
                default:
                    srBrick.sprite = lsBrickSprites[3];
                    break;
            }
        }

        public override void SetPosition(Vector2 pos)
        {
            transform.position = pos;
            textBrick = Instantiate(GameController.ins.textPrefab);
            textBrick.GetComponent<RectTransform>().anchoredPosition = GameController.ins.cam.WorldToScreenPoint(pos);
            textBrick.transform.SetParent(GameController.ins.parentText);
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
            textBrick.transform.SetParent(GameController.ins.parentText);
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
            GameController.ins.OnBurst(transform.position, i, j, type);
        }

        public override void Remove()
        {
            base.Remove();
            textBrick.gameObject.SetActive(false);
        }
    }
}