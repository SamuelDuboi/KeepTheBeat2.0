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

    public void Reset()
    {
        for (int i = 0; i < Score.Instance.cptStreak; i++)
        {
            arrows[i].SetActive(false);
        }
    }

    public void UpdateVisuel()
    {
        for (int i = 0; i < arrows.Count; i++)
        {
            arrows[i].SetActive(false);
        }

        for (int i = 0; i < Score.Instance.cptStreak; i++)
        {
            arrows[i].SetActive(true);
        }
    }

}
