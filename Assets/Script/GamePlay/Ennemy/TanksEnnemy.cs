using AudioHelm;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class TanksEnnemy : EnnemyBehavior
{
    private bool canReset;
    private int cptDead;
    public Light light1Tank, light2Tank;
    public Light lightParent;

    private void Start()
    {
        light1.color = lightParent.color;
        light2.color = lightParent.color;
    }

    private void Update()
    {
        if (cptDead > 0 && !canReset)
        {
            canReset = true;
            StartCoroutine(Reset());
        }
    }
    public override void Destroyed(int value)
    {
        cptDead++;
        if (cptDead >= 2)
        {
            Score.Instance.ScoreUp(scoreValue*(value+1));
            if (SceneManager.GetActiveScene().name != "Tuto")
                SoundDisplay.Instance.RemoveEnnemy(gameObject);
            else
                SoundDisplqyTuto.Instance.RemoveEnnemy(gameObject);
            Instantiate(explosion, transform.position, Quaternion.identity);

            Score.Instance.ScoreUp(scoreValue * value);
            GameObject poptext = Instantiate(popTextScore, poptextPosition.transform.position, Quaternion.identity);
            poptext.GetComponent<TextMeshPro>().text = (scoreValue * value).ToString();
            Destroy(gameObject);
        }
    }

    private IEnumerator Reset()
    {
        yield return new WaitForSeconds(60 / AudioHelmClock.Instance.bpm * 0.5f);
        cptDead =  0 ;
        canReset = false;
    }
}
