using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParticule : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(Destroy());
    }

    IEnumerator Destroy()
    {

        //transform.GetComponentInChildren<ParticleSystem>().startColor = 
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
}
