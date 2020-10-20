using AudioHelm;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkedEnnemy :MonoBehaviour
{
    public GameObject[] hitBox = new GameObject[2];
    public GameObject explosion;
    [HideInInspector] public GameObject[] tiles = new GameObject[2];
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
        if (tiles[0] != null)
        {
            tiles[0].GetComponent<TilesBehavior>().Off();
            tiles[1].GetComponent<TilesBehavior>().Off();
        }
        Instantiate(explosion, hitBox[0].transform.position, Quaternion.identity);
        Instantiate(explosion, hitBox[1].transform.position, Quaternion.identity);
        SoundDisplay.Instance.RemoveEnnemy(hitBox[1]);
        SoundDisplay.Instance.RemoveEnnemy(hitBox[0]);
        Destroy(gameObject);
    }
    private IEnumerator Reset()
    {

        yield return new WaitForSeconds ((60 / AudioHelmClock.GetGlobalBpm()) *0.5f);
        hitCpt = 0;
        cantReset = false;
    }
}
