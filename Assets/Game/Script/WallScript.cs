using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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