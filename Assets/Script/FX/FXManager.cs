using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FXManager : MonoBehaviour
{
    public static FXManager fxm;

    public List<GameObject> meteorsList = new List<GameObject>();
    public List<GameObject> ennemyexplosionList = new List<GameObject>();

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

    public void InstantiateExplosion(Transform spawnpos, int index)
    {
        Instantiate(ennemyexplosionList[index], spawnpos);
    }
}
