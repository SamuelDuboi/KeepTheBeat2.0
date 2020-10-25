using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMovemenet : MonoBehaviour
{
    public int life;
    public Vector3 direction;
    public float speed;
    private bool doOnce;
    private bool doOnce2;
    private bool doOnce3;

    private void FixedUpdate()
    {
        transform.Translate(direction * Time.deltaTime * speed);
        if (transform.position.z <= 40 && !doOnce)
        {
            Main.Instance.canShootBoss = true;
            doOnce = true;
            speed = 2;

        }
        if(transform.position.z >= 25 && transform.position.z <= 30 && !doOnce2)
        {
            doOnce2 = true;
            Main.Instance.BossPhaseUp();
        }
       else  if (transform.position.z >= 15 && transform.position.z <= 20 && !doOnce3)
        {
            doOnce3 = true;
            Main.Instance.BossPhaseUp();

        }
        else  if (transform.position.z <= 10)
        {
            Main.Instance.BossOverTest(life, gameObject);
        }
    }
}
