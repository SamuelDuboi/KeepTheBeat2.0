using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorSpawner : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(SpawnMeteor(Random.Range(0, 3f)));
    }

    IEnumerator SpawnMeteor(float time)
    {
        yield return new WaitForSeconds(time);
        FXManager.fxm.InstantiateMeteor(transform);
        StartCoroutine(SpawnMeteor(Random.Range(5, 10f)));
    }
}
