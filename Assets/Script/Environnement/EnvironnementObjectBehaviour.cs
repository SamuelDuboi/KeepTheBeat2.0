using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnvironnementObjectBehaviour : MonoBehaviour
{
    Transform CamPos;
    [Header ("Object")]
    public bool isObject;
    public float Scale;
    public float Multi;

    [Header("Particule")]
    public bool isParticuleEffect;

    void Start()
    {
        CamPos = Camera.main.transform;
        if (isParticuleEffect == true)
            gameObject.GetComponent<ParticleSystem>().Stop();
    }

    void Update()
    {
        if (isObject)
        {
            SoundDisplay.Instance.ScaleObject(gameObject, Scale, Multi);
            if (transform.position.z < CamPos.transform.position.z - 20)
            {
                Destroy(gameObject);
            }
        }
    }

    public void PlayParticule()
    {
        gameObject.GetComponent<ParticleSystem>().Play();
    }

    public void StopParticule()
    {
        gameObject.GetComponent<ParticleSystem>().Stop();
    }
}
