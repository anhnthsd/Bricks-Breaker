using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Script
{
    public class ItemShootBall : BaseBrick
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

        public override void OnSpawn(int hp)
        {
            colBrick.isTrigger = true;
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

        public override void SetSprite(TypeOfBrick type)
        {
        }

        public override void TakeDamage()
        {
        }

        public override void SetPosition(Vector2 pos)
        {
            transform.position = pos;
        }

        public override void Active(bool isActive)
        {
            gameObject.SetActive(isActive);
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            isOver = true;
            var ballS = col.GetComponent<BallScript>();
            if (ballS.state == StateBall.Fly)
            {
                ballS.rigi.velocity = Vector2.zero;
                Vector2 f = new Vector2(Random.Range(0.5f, 1), Random.Range(0.5f, 1)) * 450;
                ballS.rigi.AddForce(f);
            }
        }
    }
}