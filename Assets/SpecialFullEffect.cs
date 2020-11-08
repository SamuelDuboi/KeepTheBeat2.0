using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpecialFullEffect : MonoBehaviour
{
    #region Variables
    [Header("GameObject References")]
    [SerializeField] private GameObject particuleToSpawn;
    [SerializeField] private GameObject endPosition;

    public bool doOnce = false;

    public Color[] colorToChoose;
    #endregion


    private void Update()
    {
        if(Main.Instance.specialCount == Main.Instance.specialMaxValue && !doOnce)
        {
            doOnce = true;
            StartCoroutine(SpawnEffect());
        }
        else
        {
            StopAllCoroutines();
            doOnce = false;
        }
    }

    IEnumerator SpawnEffect()
    {
       GameObject effectSpawned = Instantiate(particuleToSpawn, gameObject.transform.position, Quaternion.identity);
        //Endpositions
        effectSpawned.GetComponent<DestroyAfterASec>().particuleG.GetComponent<SpecialFullParticuleBehaviour>().endPosition = endPosition;
        effectSpawned.GetComponent<DestroyAfterASec>().particuleD.GetComponent<SpecialFullParticuleBehaviour>().endPosition = endPosition;
        //Color
        int ColorChoose = Random.Range(0, 6);
        effectSpawned.GetComponent<DestroyAfterASec>().particuleD.GetComponent<Image>().color = colorToChoose[ColorChoose];
        effectSpawned.GetComponent<DestroyAfterASec>().particuleG.GetComponent<Image>().color = colorToChoose[ColorChoose];

       yield return new WaitForSeconds(Random.Range(0.1f,0.3f));
        StartCoroutine("SpawnEffect");
    }

}
