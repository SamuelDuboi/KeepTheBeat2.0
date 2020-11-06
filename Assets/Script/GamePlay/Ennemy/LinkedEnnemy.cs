using AudioHelm;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class LinkedEnnemy :MonoBehaviour
{
    public GameObject[] hitBox = new GameObject[2];
    public GameObject explosion;
    [HideInInspector] public int hitCpt;
    private bool cantReset;

    [SerializeField] private GameObject enemy0;
    [SerializeField] private GameObject enemy1;

    public int scoreValue;
    public GameObject popTextScore;
    public GameObject poptextPosition0;
    public GameObject poptextPosition1;

    void Update()
    {
        if(hitCpt == 1 && !cantReset) 
        {
            cantReset = true;
            StartCoroutine(Reset());
        }
    }

    public void Hitted()
    {
        hitCpt++; 
        if (hitCpt >= 2)
        {
            Score.Instance.ScoreUp(hitBox[1].GetComponent<EnnemyBehavior>().scoreValue);
            Score.Instance.ScoreUp(hitBox[0].GetComponent<EnnemyBehavior>().scoreValue);
            DestroyAll(true);
            
        }

    }

    public void DestroyAll(bool scoreUp)
    {
        if(scoreUp)
        {
            Score.Instance.ScoreUp(scoreValue);
        }
        GameObject poptext0 = Instantiate(enemy0.GetComponent<EnnemyBehavior>().popTextScore, poptextPosition0.transform.position, Quaternion.identity);
        poptext0.GetComponent<TextMeshPro>().text = (scoreValue).ToString();
        GameObject poptext1 = Instantiate(enemy1.GetComponent<EnnemyBehavior>().popTextScore, poptextPosition1.transform.position, Quaternion.identity);
        poptext1.GetComponent<TextMeshPro>().text = (scoreValue).ToString();

        if (hitBox[0].GetComponent<EnnemyBehavior>().tile != null)
        {
            hitBox[0].GetComponent<EnnemyBehavior>().tile.GetComponent<TilesBehavior>().Off();
            hitBox[1].GetComponent<EnnemyBehavior>().tile.GetComponent<TilesBehavior>().Off();
        }
        hitBox[0].GetComponent<EnnemyBehavior>().Destroyed(1, scoreUp);
        hitBox[1].GetComponent<EnnemyBehavior>().Destroyed(1, scoreUp);
        if (SceneManager.GetActiveScene().name != "Tuto")
            SoundDisplay.Instance.RemoveEnnemy(gameObject);
        else 
            SoundDisplqyTuto.Instance.RemoveEnnemy(gameObject);
        Destroy(gameObject);
    }
    private IEnumerator Reset()
    {

        yield return new WaitForSeconds ((60 / AudioHelmClock.Instance.bpm) *0.5f);
        hitCpt = 0;
        cantReset = false;
    }
}
