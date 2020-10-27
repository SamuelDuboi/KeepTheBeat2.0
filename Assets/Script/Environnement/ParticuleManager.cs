using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticuleManager :Singleton<ParticuleManager>
{
    public List<GameObject> effectList = new List<GameObject>();

    public void ActivateEffects()
    {
        foreach(GameObject effect in effectList)
        {
            effect.GetComponent<ParticleSystem>().Play();
        }
    }
}
