using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyText : MonoBehaviour
{
    [Range(0.1f, 1f)]
    public float deadFloat;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, deadFloat);
    }

   
}
