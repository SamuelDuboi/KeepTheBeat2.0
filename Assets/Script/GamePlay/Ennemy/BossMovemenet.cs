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
    private bool doOnce4;
    private bool doOnce5;
    private bool doOnce6;
    private bool doOnce7;

    public GameObject smallExplosion;
    private float timer;
    private void FixedUpdate()
    {
        transform.Translate(direction * Time.deltaTime * speed);
        if (doOnce)
        {
            timer += Time.deltaTime;
            if(timer >= 0.2f)
            {
                timer = 0;
                var _x = Random.Range(-1f, 1f);
                var _y = Random.Range(-1f, 1f);
                Instantiate(smallExplosion, new Vector3(transform.position.x+ _x, transform.position.y-2+_y, transform.position.z - 18),Quaternion.identity, transform);
            }
        }
        if (transform.position.z <= 45 && !doOnce)
        {
            Main.Instance.canShootBoss = true;
            doOnce = true;
            speed = 1;

        }
        if(transform.position.z >= 35 && transform.position.z <= 40 && !doOnce2)
        {
            doOnce2 = true;
            Main.Instance.BossPhaseUp();
        }
        else if (transform.position.z >= 30 && transform.position.z <= 35 && !doOnce3)
        {
            doOnce3 = true;
            Main.Instance.BossPhaseUp();
        }
        else if (transform.position.z >= 25 && transform.position.z <= 30 && !doOnce4)
        {
            doOnce4 = true;
            Main.Instance.BossPhaseUp();
        }
        else  if (transform.position.z >= 20 && transform.position.z <= 25 && !doOnce5)
        {
            doOnce5 = true;
            Main.Instance.BossPhaseUp();

        }
        if (transform.position.z >= 15 && transform.position.z <= 20 && !doOnce6)
        {
            doOnce6 = true;
            Main.Instance.BossPhaseUp();
        }
        else  if (transform.position.z <= 10)
        {
            Main.Instance.BossOverTest(life, gameObject);
        }
    }
}
