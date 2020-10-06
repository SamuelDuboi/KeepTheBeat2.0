using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{

    public Vector3 rotation;
    public float vectorSpeed;
    void FixedUpdate()
    {
        transform.Rotate(rotation * vectorSpeed * Time.deltaTime);
    }
}
