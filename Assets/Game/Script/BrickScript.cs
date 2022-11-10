using System;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Script
{
    public class BrickScript : MonoBehaviour
    {
        [SerializeField] private Collider2D _collider;
        public int hp;
        public TextMeshProUGUI txtScore;
        public TypeOfBrick typeOfBrick = TypeOfBrick.Normal;
        [SerializeField] public SpriteRenderer spriteRenderer;
        public Sprite sprite;

        private void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.CompareTag("ball"))
            {
                hp--;
                txtScore.text = hp.ToString();
                if (hp == 0)
                {
                    gameObject.SetActive(false);
                    txtScore.gameObject.SetActive(false);
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            switch (typeOfBrick)
            {
                case TypeOfBrick.ShootRandom:
                    break;
                case TypeOfBrick.DamageHorizontal:
                    break;
                case TypeOfBrick.DamageVertical:
                    break;
                case TypeOfBrick.DamageBoth:
                    break;
                default:
                    hp--;
                    txtScore.text = hp.ToString();
                    if (hp == 0)
                    {
                        gameObject.SetActive(false);
                        txtScore.gameObject.SetActive(false);
                    }

                    break;
            }
        }

        public void OnSpawn(int hp, TypeOfBrick type = TypeOfBrick.Normal)
        {
            this.hp = hp;
            typeOfBrick = type;
            if (typeOfBrick is TypeOfBrick.DamageBoth or TypeOfBrick.DamageHorizontal or TypeOfBrick.DamageVertical
                or TypeOfBrick.ShootRandom)
            {
                txtScore.text = "";
                _collider.isTrigger = true;
            }
            else txtScore.text = hp.ToString();

            switch (typeOfBrick)
            {
                case TypeOfBrick.ShootRandom:
                    spriteRenderer.sprite = sprite;
                    break;
            }
        }
    }
}

public enum TypeOfBrick
{
    Normal,
    Fixed,
    DeleteHorizontal,
    DeleteVertical,
    DeleteBoth,
    DeleteSurround,
    DamageHorizontal,
    DamageVertical,
    DamageBoth,
    ShootRandom
}