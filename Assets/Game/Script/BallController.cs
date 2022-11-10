using System;
using UnityEngine;

namespace Game.Script
{
    public class BallController : MonoBehaviour
    {
        public static BallController ins;
        public GameObject ball;
        // private bool isFly = false;

        public void CreateBall(int ballCount, Vector2 position)
        {
            GameObject newBall = Instantiate(ball);
            // newBall.transform.position = position;
            ball.transform.position = position;
        }

        public void ShootBall(Vector2 f)
        {
        }
    }
}