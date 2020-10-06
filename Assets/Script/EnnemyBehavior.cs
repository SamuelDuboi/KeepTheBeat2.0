using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyBehavior : MonoBehaviour
{
    private double bpm;
    public Vector2[] positions;
  [HideInInspector]  public int cpt;
    public int scoreValue;
    public int enemyIndex;

   public virtual void Move()
    {
        if (cpt < 5)
        {
            cpt++;
            transform.position = positions[cpt];
        }
        else
        {
            SoundDisplay.Instance.RemoveEnnemy(gameObject);
            Score.Instance.ModifierDown();
            StartCoroutine(URPManager.urpM.ColorOnBeat(enemyIndex, 1f));
            Destroy(gameObject);
        }
    }
}
