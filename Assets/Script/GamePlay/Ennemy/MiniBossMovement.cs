using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniBossMovement : MonoBehaviour
{
    public int life;
    public Vector3 direction;
    public float speed;
    private bool doOnce;

    public GameObject smallExplosion;
    private float timer;
    private void FixedUpdate()
    {
        transform.Translate(direction * Time.deltaTime * speed);
        if (doOnce)
        {
            timer += Time.deltaTime;
            if (timer >= 0.2f)
            {
                timer = 0;
                var _x = Random.Range(-1f, 1f);
                var _y = Random.Range(-1f, 1f);
                Instantiate(smallExplosion, new Vector3(transform.position.x + _x, transform.position.y + _y, transform.position.z - 18), Quaternion.identity, transform);
            }
        }
        if (transform.position.z <= 30 && !doOnce)
        {
            Main.Instance.canShootMiniBoss = true;

            doOnce = true;
            speed = 1.8f;

        }
        if (transform.position.z <= 10)
        {
            Main.Instance.BossOverTest(life, gameObject);
        }
    }
}
