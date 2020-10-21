using AudioHelm;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkedEnnemy :MonoBehaviour
{
    public GameObject[] hitBox = new GameObject[2];
    public GameObject explosion;
    [HideInInspector] public int hitCpt;
    private LineRenderer trail;
    private bool cantReset;

    private void Start()
    {
        trail =GetComponent<LineRenderer>();
    }
    void Update()
    {
        trail.SetPosition(0, hitBox[0].transform.position);
        trail.SetPosition(1, hitBox[1].transform.position);
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
            DestroyAll();
            
        }

    }

    public void DestroyAll()
    {
        if (hitBox[0].GetComponent<EnnemyBehavior>().tile != null)
        {
            hitBox[0].GetComponent<EnnemyBehavior>().tile.GetComponent<TilesBehavior>().Off();
            hitBox[1].GetComponent<EnnemyBehavior>().tile.GetComponent<TilesBehavior>().Off();
        }
        hitBox[0].GetComponent<EnnemyBehavior>().Destroyed();
        hitBox[1].GetComponent<EnnemyBehavior>().Destroyed();
        SoundDisplay.Instance.RemoveEnnemy(gameObject);
        Destroy(gameObject);
    }
    private IEnumerator Reset()
    {

        yield return new WaitForSeconds ((60 / AudioHelmClock.GetGlobalBpm()) *0.5f);
        hitCpt = 0;
        cantReset = false;
    }
}
