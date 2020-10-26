using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexSpawner : MonoBehaviour
{
    public GameObject HexCircle;
    public bool canSpawn;
    void Start()
    {
        InvokeRepeating("SpawnCircle", 0.1f, 0.1f);
    }

    void Update()
    {
        if (canSpawn == true)
        {

        } 
    }

    void SpawnCircle()
    {
        Instantiate(HexCircle,transform.position,Quaternion.identity);
    }
}
