using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticuleManager :Singleton<ParticuleManager>
{
    public List<GameObject> effectList = new List<GameObject>();

    public List<GameObject> SpecialEffectsOnce = new List<GameObject>();
    public List<GameObject> SpecialEffectsLoop = new List<GameObject>();
    protected override void Awake()
    {
        base.Awake();
    }
    public void Start()
    {
        foreach (GameObject effect in effectList)
        {
            effect.GetComponent<ParticleSystem>().Stop();
        }
    }

    public void ActivateEffects()
    {
        foreach(GameObject effect in effectList)
        {
            effect.GetComponent<ParticleSystem>().Play();
        }
    }

    public void PlayEffectsOnce()
    {
        foreach (GameObject effect in SpecialEffectsOnce)
        {
            effect.GetComponent<ParticleSystem>().Play();
        }
    }

    public void PlayEffectsLoop()
    {
        foreach (GameObject effect in SpecialEffectsLoop)
        {
            effect.GetComponent<ParticleSystem>().Play();
        }
    }

    public void StopEffectsLoop()
    {

        foreach (GameObject effect in SpecialEffectsLoop)
        {
            effect.GetComponent<ParticleSystem>().Stop();
        }
    }
}
