using System;
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
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.CompareTag("ball"))
            {
                var fx = Instantiate(fxBrick);
                fx.transform.position = transform.position;
                OnDamge();
                Destroy(fx, 0.3f);
            }
        }

        public void OnDamge()
        {
            hpBrick--;
            textBrick.text = hpBrick.ToString();
            if (hpBrick <= 0)
            {
                gameObject.SetActive(false);
                textBrick.gameObject.SetActive(false);
            }
        }
    }
}