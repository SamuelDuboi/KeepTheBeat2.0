using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterGameUiBehavior : MonoBehaviour
{
   
    // Update is called once per frame
    void Update()
    {
        if(transform.localPosition.z > -1500)
        transform.Translate(Vector3.back*3, Space.World);
    }
}
