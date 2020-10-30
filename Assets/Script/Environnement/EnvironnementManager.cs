using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironnementManager : Singleton<EnvironnementManager>
{
    public int step;
    public List<GameObject> Phase1, Phase2, Phase3;
   
    void Start()
    {
        LunchPhase();
    }


    public void LunchPhase()
    {
        step++;
        if (step == 1)
        {
            ResetFx();
            ActivatePhase1();
        }
        if (step == 2 )
        {
            ResetFx();
            ActivatePhase1();
        }
        if (step == 3 )
        {
            ResetFx();
            ActivatePhase1();
        }
    }

    void ActivatePhase1()
    {
        
        foreach (GameObject envi in Phase1)
        {
            if(envi.GetComponent<EnvironnementObjectBehaviour>() != null)
            {
                if (envi.GetComponent<EnvironnementObjectBehaviour>().isParticuleEffect == true)
                {
                    envi.GetComponent<EnvironnementObjectBehaviour>().PlayParticule();
                }
            }

            if(envi.GetComponent<EnvironnementSpawner>()!= null)
            {
                envi.GetComponent<EnvironnementSpawner>().canSpawn = true;
            }
        }
    }

    public void ResetFx()
    {
        foreach (GameObject envi in Phase1)
        {
            if (envi.GetComponent<EnvironnementObjectBehaviour>() != null)
            {
                if (envi.GetComponent<EnvironnementObjectBehaviour>().isParticuleEffect == true)
                {
                    envi.GetComponent<EnvironnementObjectBehaviour>().StopParticule();
                }
            }
            if (envi.GetComponent<EnvironnementSpawner>() != null)
            {
                envi.GetComponent<EnvironnementSpawner>().canSpawn = false;
            }
        } 
        
        foreach (GameObject envi in Phase2)
        {
            if (envi.GetComponent<EnvironnementObjectBehaviour>() != null)
            {
                if (envi.GetComponent<EnvironnementObjectBehaviour>().isParticuleEffect == true)
                {
                    envi.GetComponent<EnvironnementObjectBehaviour>().StopParticule();
                }
            }
            if (envi.GetComponent<EnvironnementSpawner>() != null)
            {
                envi.GetComponent<EnvironnementSpawner>().canSpawn = false;
            }
        }  

        foreach (GameObject envi in Phase3)
        {
            if (envi.GetComponent<EnvironnementObjectBehaviour>() != null)
            {
                if (envi.GetComponent<EnvironnementObjectBehaviour>().isParticuleEffect == true)
                {
                    envi.GetComponent<EnvironnementObjectBehaviour>().StopParticule();
                }
            }
            if (envi.GetComponent<EnvironnementSpawner>() != null)
            {
                envi.GetComponent<EnvironnementSpawner>().canSpawn = false;
            }
        }
    }
}
