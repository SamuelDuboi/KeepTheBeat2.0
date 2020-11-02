using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PostProcessManager : MonoBehaviour
{
    public static PostProcessManager post;
    Animator animator;
    public List<VolumeProfile> FXprofile = new List<VolumeProfile>();

    void Start()
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
}
