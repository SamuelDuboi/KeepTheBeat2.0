using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class LifeManager : Singleton<LifeManager>
{
    public TextMeshProUGUI lifeText;
    public int lifes;
    public static int life;
    public GameObject gameOver;
    public Image fade;

    private void Start()
    {
        life = lifes;
        lifeText.text = "Life: " + life.ToString();
    }
    public void TakeDamage( AudioMixer mixer)
    {
        life--;
        mixer.SetFloat("MainVolume", -5);
        if(life <= 0)
        {
            gameOver.SetActive(true);
            Time.timeScale = 0;

        }
        else
        {
            lifeText.text = "Life: " + life.ToString();
            StartCoroutine(Fade(mixer));
        }

    }

    IEnumerator Fade(AudioMixer mixer)
    {
        for (int i = 0; i < 100; i++)
        {
            fade.color += new Color(0, 0, 0, 0.01f);
            yield return new WaitForSeconds(0.005f);
        }
        for (int i = 0; i < 100; i++)
        {
            fade.color -= new Color(0, 0, 0, 0.01f);
            yield return new WaitForSeconds(0.002f);
        }
        mixer.SetFloat("MainVolume", 0);

    }


    public void GameOver()
    {
        gameOver.SetActive(true);
        //Application.Quit();
    }
}
