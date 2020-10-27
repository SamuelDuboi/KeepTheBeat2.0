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

        if (isParticuleEffect)
        {

        }
 
    }
}
