using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeManager : Singleton<LifeManager>
{
    public Text lifeText;
    public int lifes;
    public static int life;
    public GameObject gameOver;
    public Image fade;

    private void Start()
    {
        life = lifes;
        lifeText.text = "Life: " + life.ToString();
    }
    public void TakeDamage(AudioSource[] audioSource, AudioSource mainSource)
    {
        life--;
        mainSource.volume = mainSource.volume / 2;
        if(life <= 0)
        {
            gameOver.SetActive(true);
            Time.timeScale = 0;
            foreach (AudioSource sound in audioSource)
            {
                sound.volume = sound.volume / 2;
            }
        }
        else
        {
            float[] _volume = new float[audioSource.Length];
            for (int i = 0; i < _volume.Length-1; i++)
            {
                _volume[i] = audioSource[i].volume;
                audioSource[i].volume = audioSource[i].volume / 2;
            }
            lifeText.text = "Life: " + life.ToString();
            StartCoroutine(Fade(_volume,audioSource, mainSource));
        }

    }

    IEnumerator Fade(float[] _volume, AudioSource[] audioSource, AudioSource mainSource)
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
        for (int i = 0; i < _volume.Length - 1; i++)
        {
            audioSource[i].volume = _volume[i];
        }
        mainSource.volume = mainSource.volume* 2;

    }
}
