using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIEffects : MonoBehaviour
{
    [SerializeField] private List<GameObject> arrows = new List<GameObject>();

    [SerializeField] private int success = 0;

 
    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
            arrows.Add(child.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //lancer l'update que quand necessaire après
       
        success = Score.Instance.cptStreak;
        if (success == 0)
        {
            arrows[0].SetActive(false);
        }
        else if (success == 1)
        {
            arrows[0].SetActive(true);
            arrows[1].SetActive(false);
        }
        else if (success == 2)
        {
            arrows[1].SetActive(true);
            arrows[2].SetActive(false);
        }
        else if (success == 3)
        {
            arrows[2].SetActive(true);
            arrows[3].SetActive(false);
        }
        else if (success == 4)
        {
            arrows[3].SetActive(true);
            arrows[4].SetActive(false);
        }
        else if (success == 5)
        {
            arrows[4].SetActive(true);
            arrows[5].SetActive(false);
        }
        else if (success == 6)
        {
            arrows[5].SetActive(true);

        }
    }

    public void Reset()
    {
        for (int i = 0; i < arrows.Count; i++)
        {
            arrows[i].SetActive(false);
        }
    }

}
