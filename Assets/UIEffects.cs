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
        UpdateVisuel();
        success = Score.Instance.cptStreak;
    }

    public void UpdateVisuel()
    {
        for (int i = 0; i < success; i++)
        {
            arrows[i].SetActive(true);
        }

        for (int i = 0; i < success; i++)
        {
            arrows[i].SetActive(true);
        }
    }
}
