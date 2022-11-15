using System;
using TMPro;
using UnityEngine;

namespace Game.Script
{
    public class BurstBrick : BaseBrick
    {
        public int hpBrick;
        public TextMeshProUGUI textBrick;
        public GameObject fxBrick;
        public GameObject fxBurstHor;
        public GameObject fxBurstVer;

        public override void OnSpawn(int hp)
        {
            hpBrick = hp;
            textBrick.text = hp.ToString();
        }

        public override void OnDelete()
        {
            gameObject.SetActive(false);
            textBrick.gameObject.SetActive(false);
            GameController.ins.DelBrick(i, j);
        }

        public override void SetPosition(Vector2 pos)
        {
            transform.position = pos;
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
                OnDamage();
                Destroy(fx, 0.3f);
            }
        }

        public override void OnDamage()
        {
            hpBrick--;
            textBrick.text = hpBrick.ToString();
            if (hpBrick == 0)
            {
                OnDelete();
                GameController.ins.OnBurst(j, i);
                var fx = Instantiate(fxBurstHor);
                fx.transform.position = transform.position;
                Destroy(fx, 0.3f);
            }
        }
    }
}