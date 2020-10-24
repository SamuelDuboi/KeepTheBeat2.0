using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleScreen : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 1; i < Display.displays.Length; i++)
        {
            Display.displays[i].Activate(1920,1080,60);
        }
    }
}
