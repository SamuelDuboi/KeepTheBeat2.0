using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyBehavior : MonoBehaviour
{
    private double bpm;
    public Vector3[] positions;
  [HideInInspector]  public int cpt;
    public int scoreValue;
    [HideInInspector] public GameObject tile;


   public virtual void Move()
    {
        if (cpt < 5)
        {
            cpt++;
            if (tile != null)
                tile.GetComponent<TilesBehavior>().Off();
            transform.position = positions[cpt];
            RaycastHit hit;
            Physics.Raycast(transform.position, Vector3.down, out hit);
            if (hit.collider != null && hit.collider.GetComponent<TilesBehavior>() != null)
            {
                tile = hit.collider.gameObject;
                tile.GetComponent<TilesBehavior>().On();
            }
            else
                Debug.Log(hit.collider);


        }
        else
        {
            tile.GetComponent<TilesBehavior>().Off();
            SoundDisplay.Instance.RemoveEnnemy(gameObject);    
            Score.Instance.ModifierDown();
            Destroy(gameObject);
        }
    }

}
