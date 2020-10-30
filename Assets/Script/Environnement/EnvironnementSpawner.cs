using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironnementSpawner : MonoBehaviour
{
    public List<GameObject> objectToSpawn = new List<GameObject>();
    public bool canSpawn;
    public bool key = true;

    void Update()
    {
        if (canSpawn == true && key == true)
        {
            StartCoroutine(Spawn());
        }
    }

    IEnumerator Spawn()
    {
        key = false;
        Instantiate(objectToSpawn[Random.Range(0,objectToSpawn.Count)], transform.position + new Vector3(Random.Range(-10,10),Random.Range(2,10),0), Quaternion.identity);
        yield return new WaitForSeconds(Random.Range(2f, 4f));
        key = true;
    }
}
