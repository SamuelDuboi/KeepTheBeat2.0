using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TutoText : MonoBehaviour
{
    private TextMeshProUGUI text;
    public string[] texts = new string[8];
  
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    public void NextText(int txtCpt)
    {
        text.text = texts[txtCpt];
        StartCoroutine(FadeIn());
    }
    
    IEnumerator FadeIn()
    {
        for (float i = 0; i < 255; i++)
        {
            text.color = new Color(1, 1, 1, i / 255);
            yield return new WaitForSeconds(0.01f);
        }
}
    IEnumerator FadeOut()
    {
        for (float i = 255; i < 0; i--)
        {
            text.color = new Color(1, 1, 1, i / 255);
            yield return new WaitForSeconds(0.008f);
        }
        text.text = string.Empty;
    }
    public void ClearText()
    {
        StartCoroutine(FadeOut());
    }
}
