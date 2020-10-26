using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class LifeManager : Singleton<LifeManager>
{

    public GameObject animationRender;
    public Animator hpAnimator;

    public int lifes;
    public static int life;
    public GameObject gameOver;
    public Image fade;

    private void Start()
    {

        hpAnimator = gameObject.GetComponent<Animator>();
        
        life = lifes;
      
    }
    public void TakeDamage(AudioMixer mixer)
    {
        TriggerAnim();

        life--;

        if (life <= 0)
        {
            Score.Instance.EndScene(false);
        }
        mixer.SetFloat("MainVolume", -5);
        
        StartCoroutine(Fade(mixer));
        

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

    public void TriggerAnim()
    {
        if (life == 5)
        {
            hpAnimator.SetTrigger("HP5");
        }
        else if (life == 4)
        {
            hpAnimator.SetTrigger("HP4");
        }
        else if (life == 3)
        {
            hpAnimator.SetTrigger("HP3");
        }
        else if (life == 2)
        {
            hpAnimator.SetTrigger("HP2");
        }
        else if (life == 1)
        {
            hpAnimator.SetTrigger("HP1");
        }

    }
}
