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
            doOnce = true;
            speed = 2;

        }
        if(transform.position.z <= -150)
        {
            Main.Instance.MiniBossOverTest(life, gameObject);
        }
    }
}
