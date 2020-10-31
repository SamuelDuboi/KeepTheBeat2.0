using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TutoText : MonoBehaviour
{
    private TextMeshProUGUI text;
    public string[] texts = new string[8];
    private int txtCpt;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    public void NextText()
    {
        text.text = texts[txtCpt];
        txtCpt++;
    }
    

    public void ClearText()
    {
        text.text = string.Empty;
    }
}
