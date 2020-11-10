using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterASec : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject particuleG;
    public GameObject particuleD;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }

}
