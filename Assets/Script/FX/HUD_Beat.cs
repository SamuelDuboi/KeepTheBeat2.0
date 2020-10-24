using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AudioHelm;

public class HUD_Beat : MonoBehaviour
{
    private double timer;
    private double timePreviousBeat;

    // Start is called before the first frame update
    void Start()
    {
        timer = AudioHelmClock.GetGlobalBeatTime() - timePreviousBeat;
    }

    // Update is called once per frame
    void Update()
    {
       
            float scaleFactor = 80 + (float)timer / (60 / AudioHelmClock.GetGlobalBpm()) * 20;
            gameObject.transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
        
    }
}
