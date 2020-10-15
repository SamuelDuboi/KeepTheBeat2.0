using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class TESTEnemyInstancier : MonoBehaviour
{

    public GameObject enemy;
    public Transform destination;
    GameObject newEn;

    void Start()
    {
       newEn = Instantiate(enemy, transform);
    }


    private void FixedUpdate()
    {
        newEn.transform.Translate(new Vector3(destination.position.x - newEn.transform.position.x, destination.position.y - newEn.transform.position.y, destination.position.z - newEn.transform.position.z) * Time.deltaTime);
    }

    void Travel()
    {
        StartCoroutine(DestroyPyr());
    }

    IEnumerator DestroyPyr()
    {
        yield return new WaitForSeconds(1f);
        Destroy(enemy);
    }
}
