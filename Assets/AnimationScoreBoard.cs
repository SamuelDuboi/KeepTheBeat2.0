using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationScoreBoard : MonoBehaviour
{
    [SerializeField] private GameObject Cadre;
    [SerializeField] private GameObject Title;
    [SerializeField] private GameObject Headlines;
    [SerializeField] private GameObject Values;
    [SerializeField] private GameObject Top10;
    [SerializeField] private GameObject PressButton;
   


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(PopLeaderBoard());   
    }

    IEnumerator PopLeaderBoard()
    {
        Cadre.SetActive(true);
        yield return new WaitForSeconds(1f);
        Title.SetActive(true);
        yield return new WaitForSeconds(1f);
        Headlines.SetActive(true);
        yield return new WaitForSeconds(1f);
        Values.SetActive(true);
        yield return new WaitForSeconds(0.5f);

        
    }
}
