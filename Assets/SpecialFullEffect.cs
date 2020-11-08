using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialFullEffect : MonoBehaviour
{
    #region Variables
    [Header("GameObject References")]
    [SerializeField] private GameObject particuleToSpawn;
    [SerializeField] private GameObject endPosition;

    public bool doOnce = false;
    #endregion


    private void Update()
    {
        if(Main.Instance.specialCount == Main.Instance.specialMaxValue && !doOnce)
        {
            print("la");
            doOnce = true;
            StartCoroutine(SpawnEffect());
        }
    }

    IEnumerator SpawnEffect()
    {
        GameObject effectSpawned = Instantiate(particuleToSpawn, transform.position, Quaternion.identity);
        effectSpawned.transform.position = transform.position;
        effectSpawned.GetComponentInChildren<SpecialFullParticuleBehaviour>().endPosition = endPosition; 
        yield return new WaitForSeconds(Random.Range(0.1f,0.5f));
        StartCoroutine("SpawnEffect");
    }

}
