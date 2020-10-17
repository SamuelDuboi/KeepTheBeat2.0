using AudioHelm;
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
    [Range(0, 100)]
    public float speed;
    public GameObject child;
    private bool cantMove;
    private void Update()
    {
        if(!cantMove)
        transform.position = Vector3.MoveTowards(transform.position, positions[0], speed*AudioHelmClock.GetGlobalBpm());
    }


    public virtual void Move()
    {
        if (!cantMove)
        {
            cantMove = true;
            child.SetActive(false);
        }
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
