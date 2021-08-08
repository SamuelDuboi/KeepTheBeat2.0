using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PostProcessManager : MonoBehaviour
{
    public static PostProcessManager post;
    Animator animator;

    public List<VolumeProfile> FXprofile = new List<VolumeProfile>();

    void Awake()
    {
        post = this;
        animator = gameObject.GetComponent<Animator>();
    }

    public void ActivatePostProcess (int index)
    {

        gameObject.GetComponent<Volume>().profile = FXprofile[index];
        animator.SetTrigger("Activate");
    }

    public void DeactivatePostProcess()
    {
        animator.SetTrigger("Deactivate");
    }

    public void ActivatePostProcessInChild(int index)
    {
        gameObject.transform.GetChild(0).GetComponent<Volume>().profile = FXprofile[index];
    }
}

public enum postProcess { Damage, BossInc, SpecialPower, BPM_80, BPM_90, BPM_100, EndOfGame,Transition };
