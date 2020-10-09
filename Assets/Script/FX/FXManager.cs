using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FXManager : MonoBehaviour
{
    public static FXManager fxm;

    public List<GameObject> meteorsList = new List<GameObject>();

    void Awake()
    {
        fxm = this;
    }

    void Update()
    {
        
    }

    public void InstantiateMeteor(Transform spawnPos)
    {
        Instantiate(meteorsList[Random.Range(0, meteorsList.Count)], spawnPos);
    }
}
