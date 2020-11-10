using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialFullParticuleBehaviour : MonoBehaviour
{
    public GameObject endPosition;

    

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(0, 2, 0); 

        if(transform.position.y >= endPosition.transform.position.y)
        {
            Destroy(gameObject);
        }

    }

   

}
