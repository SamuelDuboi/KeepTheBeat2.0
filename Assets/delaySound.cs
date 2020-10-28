using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class delaySound : MonoBehaviour
{

    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine("WaitStart");
    }

    // Update is called once per frame
    void Update()
    { 
        
    }

    IEnumerator WaitStart()
    {
        yield return new WaitForSecondsRealtime(1f);
        audioSource.Play();
    }
}
