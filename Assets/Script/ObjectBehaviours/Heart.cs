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
        double playRate = 60 / AudioHelmClock.Instance.bpm;
        animator.speed = 1 / (float)playRate;
        //Debug.Log("animator speed =" + animator.speed);
        }
    }

    void HeartRate()
    {
        if (AudioHelmClock.Instance.bpm > 0 && !doOnce)
        {
            animator.enabled = true;
            doOnce = true;
        }
    }
}
