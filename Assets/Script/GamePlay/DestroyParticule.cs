using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParticule : MonoBehaviour
{
    IEnumerator Start()
    {

        //transform.GetComponentInChildren<ParticleSystem>().startColor = 
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
