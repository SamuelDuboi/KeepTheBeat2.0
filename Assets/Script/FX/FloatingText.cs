using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FloatingText : MonoBehaviour
{
    [SerializeField] private GameObject popPosition;
    [SerializeField] private TextMeshPro popText;
    [Range(0.3f, 3f)]
    [SerializeField] private float deadFloat;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TextMeshPro thisText = Instantiate(popText, popPosition.transform.position, Quaternion.identity);

            //Destroy(thisText, deadFloat);
        }
    }
}
