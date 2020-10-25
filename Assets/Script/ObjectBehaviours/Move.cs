using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public int life;
    public Vector3 direction;
    public float speed;
    private bool doOnce;

    private void FixedUpdate()
    {
        transform.Translate(direction * Time.deltaTime * speed);   
        if(transform.position.z <=30 && !doOnce)
        {
            Main.Instance.canShootMiniBoss = true;

            doOnce = true;
            speed = 1.8f;

        }
        if(transform.position.z <= 10)
        {
            Main.Instance.BossOverTest(life, gameObject);
        }
    }
}
