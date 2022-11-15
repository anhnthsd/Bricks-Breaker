using TMPro;
using UnityEngine;

namespace Game.Script
{
    public abstract class BaseBrick : MonoBehaviour
    {
        public SpriteRenderer srBrick;
        public Collider2D colBrick;
        public int i;
        public int j;

        public abstract void OnSpawn(int hp);
        public abstract void OnDelete();
        public abstract void OnDamage();
        public abstract void SetPosition(Vector2 pos);
    }
}