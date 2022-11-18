using System;
using TMPro;
using UnityEngine;

namespace Game.Script
{
    public abstract class BaseBrick : MonoBehaviour
    {
        public TypeOfBrick typeOfBrick;
        public SpriteRenderer srBrick;
        public Collider2D colBrick;
        public int i;
        public int j;

        public abstract void OnSpawn(int hp);
        public abstract void SetSprite(TypeOfBrick type);
        //public abstract void TakeItemBurst();
        public abstract void TakeDamage();
        public abstract void SetPosition(Vector2 pos);
        public abstract void UpdatePosition(Vector2 pos);

        public virtual void OnEndTurn()
        {
            
        }

        public bool CanDieOnBottom()
        {
            switch (typeOfBrick)
            {
                case TypeOfBrick.Normal:
                case TypeOfBrick.Triangle:
                case TypeOfBrick.DeleteHorizontal:
                case TypeOfBrick.DeleteVertical:
                case TypeOfBrick.DeleteBoth:
                case TypeOfBrick.DeleteSurround:
                    return true;
                case TypeOfBrick.Empty:
                case TypeOfBrick.AddBall:
                case TypeOfBrick.DamageHorizontal:
                case TypeOfBrick.DamageVertical:
                case TypeOfBrick.DamageBoth:
                case TypeOfBrick.ShootRandom:
                    return false;
                
            }

            return false;
        }

        public virtual void DestroyBrick()
        {
            gameObject.SetActive(false);
            BrickController.ins.DelBrick(i, j);
        }
    }
}