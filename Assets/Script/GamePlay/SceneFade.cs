using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneFade : MonoBehaviour
{

    IEnumerator Start()
    {
        var _image = GetComponent<Image>();
        for (float i = 255; i>100; i--)
        {
            _image.color = new Color(0, 0, 0, i / 255);
            yield return new WaitForSeconds(0.01f);
        }
        if (SceneManager.GetActiveScene().name == "Tuto")
        {
            SoundDisplqyTuto.Instance.canStart = true;
            foreach (var spawn in MainTuto.Instance.positionEnd)
            {
                spawn.GetComponent<Spawner>().ApppearAll();
            }
        }
        else if (SceneManager.GetActiveScene().name == "Main")
        {
            SoundDisplay.Instance.canStart = true;
            foreach (var spawn in Main.Instance.positionEnd)
            {
                spawn.GetComponent<Spawner>().ApppearAll();
            }
        }
        for (float i = 100; i > 0; i--)
        {
            _image.color = new Color(0, 0, 0, i / 255);
            yield return new WaitForSeconds(0.01f);
        }
    }
       
}
