using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Rendering;
using UnityEngine.Rendering.Universal;

public class URPManager : MonoBehaviour
{
    public static URPManager urpM;
    Camera MainCam;
    UniversalAdditionalCameraData cam;

    void Start()
    {
        urpM = this;
        MainCam = Camera.main;
    }

    /*void Update()
    {
        if (Input.GetKey("a"))
        {
            if(locker == false)
            StartCoroutine(ColorOnBeat(1, 1));
        }
        if (Input.GetKey("z"))
        {
            if (locker == false)
                StartCoroutine(ColorOnBeat(2, 1));
        }
        if (Input.GetKey("e"))
        {
            if (locker == false)
                StartCoroutine(ColorOnBeat(3, 1));
        }
        if (Input.GetKey("r"))
        {
            if (locker == false)
                StartCoroutine(ColorOnBeat(4, 1));
        }
        if (Input.GetKey("t"))
        {
            if (locker == false)
                StartCoroutine(ColorOnBeat(5, 1));
        }
        if (Input.GetKey("y"))
        {
            if (locker == false)
                StartCoroutine(ColorOnBeat(6, 1));
        }

    }*/

    public IEnumerator ColorOnBeat(int index, float time)
    {
        cam = MainCam.GetUniversalAdditionalCameraData();
        cam.SetRenderer(index);
        yield return new WaitForSeconds(time);
        cam.SetRenderer(0); 
    }
}
