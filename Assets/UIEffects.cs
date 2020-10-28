using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIEffects : MonoBehaviour
{
    [SerializeField] private List<GameObject> arrows = new List<GameObject>();

    [SerializeField] private int success = 0;

    [SerializeField] private bool needToReset = false;
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
    }

    public void UpdateVisuel()
    {
        for (int i = 0; i < success; i++)
        {
            arrows[i].SetActive(true);
        }

        if (needToReset)
        {
            needToReset = false;
            success = 0;

            for (int i = 0; i < arrows.Count; i++)
            {
                arrows[i].SetActive(false);
            }
        }
    }
}
