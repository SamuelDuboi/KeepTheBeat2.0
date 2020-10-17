using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AudioHelm;

public class Heart : MonoBehaviour
{
    private bool doOnce;
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        animator.enabled = false;
    }

    void Update()
    {
        HeartRate();
        if (doOnce)
        {
        float playRate = 60 / AudioHelmClock.GetGlobalBpm();
        animator.speed = 1 / playRate;
        Debug.Log("animator speed =" + animator.speed);
        Debug.Log("Bpm = " + AudioHelmClock.GetGlobalBpm());
        }
    }

    void HeartRate()
    {
        if (AudioHelmClock.GetGlobalBpm() > 0 && !doOnce)
        {
            animator.enabled = true;
            doOnce = true;
        }
    }
}
