using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnHeart : MonoBehaviour
{
    public GameObject particule;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return new WaitForSeconds(1f);
        particule.SetActive(true);
        yield return new WaitForSeconds(4.5f);
        GetComponent<MeshRenderer>().enabled = true;
    }


}
