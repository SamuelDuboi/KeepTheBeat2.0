using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnvironnementObjectBehaviour : MonoBehaviour
{
    Transform CamPos;
    void Start()
    {
        CamPos = Camera.main.transform;
    }

    void Update()
    {
        if(transform.position.z < CamPos.transform.position.z - 20)
        {
            Destroy(gameObject);
        }
    }
}
