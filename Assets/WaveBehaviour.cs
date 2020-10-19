using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveBehaviour : MonoBehaviour
{

    public Transform[] allChildren;
    public GameObject LootParent;

    public bool needToSpawn = false;

    // Start is called before the first frame update
    void Start()
    {
        allChildren = LootParent.GetComponentsInChildren<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (needToSpawn)
        {
            foreach (Transform child in allChildren)
            {
                child.gameObject.SetActive(true);
            }
        }
    }

}
