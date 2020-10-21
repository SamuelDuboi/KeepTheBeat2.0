using AudioHelm;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TanksEnnemy : EnnemyBehavior
{
    private bool canReset;
    private int cptDead;

    private void Update()
    {
        if (cptDead > 0 && !canReset)
        {
            canReset = true;
            StartCoroutine(Reset());
        }
    }
    public override void Destroyed()
    {
        cptDead++;
        if (cptDead >= 2)
        {
            Score.Instance.ScoreUp(scoreValue);
            SoundDisplay.Instance.RemoveEnnemy(gameObject);
            Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    private IEnumerator Reset()
    {
        yield return new WaitForSeconds((60 / AudioHelmClock.GetGlobalBpm()) * 0.5f);
        cptDead =  0 ;
        canReset = false;
    }
}
