using UnityEngine;

namespace Game.Script
{
    public class WallScript : MonoBehaviour
    {
        public GameObject fxWall;

        private void OnCollisionEnter2D(Collision2D col)
        {
            var fx = Instantiate(fxWall);
            fx.transform.position = transform.position;
            Destroy(fx, 0.3f);
        }
    }
}