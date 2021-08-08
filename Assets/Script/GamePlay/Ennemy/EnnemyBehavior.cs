using AudioHelm;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class EnnemyBehavior : MonoBehaviour
{
    [Header("GameObject Ref")]
    public GameObject poptextPosition;
    public GameObject explosion;
    public GameObject popTextScore;
    public GameObject child;
    [HideInInspector] public GameObject tile;

    [Header("Values")]
    private double bpm;
    public Vector3[] positions;
    [HideInInspector]  public int cpt;
    public int scoreValue;
    [Range(0, 100)]
    public float speed;
    [HideInInspector] public bool cantMove;
    [HideInInspector] public bool turnOnTrail;
    int type;

    public bool hitted;

    [HideInInspector] public Color row1;
    [HideInInspector] public Color row2;
    public Light light1;
    public Light light2;

    public virtual void Update()
    {
        if (!cantMove)
        {
            transform.position = Vector3.MoveTowards(transform.position, positions[0], speed* (float)AudioHelmClock.Instance.bpm);
            RaycastHit hit;
            Physics.Raycast(transform.position, Vector3.down, out hit);
            if (hit.collider != null && hit.collider.GetComponent<TilesBehavior>() != null)
            {
                tile = hit.collider.gameObject;
                tile.GetComponent<TilesBehavior>().On();
            }
        }
        if (gameObject.tag == "TPEnnemy" && cpt % 2 == 0)
        {
            light1.color = row2;
            light2.color = row1;
        }
    }


    public virtual void Move()
    {
        if (!cantMove)
        {
            cantMove = true;
            if(!turnOnTrail)
            child.SetActive(false);


        }
        if (cpt < 5)
        {
            cpt++;
            if (tile != null)
                tile.GetComponent<TilesBehavior>().Off();
            transform.position = positions[cpt];
            if (gameObject.tag == "TPEnnemy" && cpt%2 ==0)
            {
                light1.color = row2;
                light2.color = row1;
            }
            else if (gameObject.tag == "TPEnnemy" && cpt % 2 != 0)
            {
                light1.color = row1;
                light2.color = row2;
            }
            RaycastHit hit;
            Physics.Raycast(transform.position, Vector3.down, out hit);
            if (hit.collider != null && hit.collider.GetComponent<TilesBehavior>() != null)
            {
                tile = hit.collider.gameObject;
                tile.GetComponent<TilesBehavior>().On();
            }
          /*  else
                Debug.Log(hit.collider);*/


        }
        else
        {
            if (gameObject.tag != "LinkedEnnemy")
            {
                if (tile != null)
                    tile.GetComponent<TilesBehavior>().Off();
                if (SceneManager.GetActiveScene().name != "Tuto")
                    SoundDisplay.Instance.RemoveEnnemy(gameObject);
                else
                    SoundDisplqyTuto.Instance.RemoveEnnemy(gameObject);
                Score.Instance.ModifierDown();
                Destroy(gameObject);

            }
            else
            {
                Score.Instance.ModifierDown();
                GetComponentInParent<LinkedEnnemy>().DestroyAll(false);
            }
        }
    }

    public virtual void Destroyed(int time, bool scorUp)
    {
        if(scorUp)
            Score.Instance.ScoreUp(scoreValue * time);
            GameObject poptext = Instantiate(popTextScore, poptextPosition.transform.position, Quaternion.identity);
            poptext.GetComponent<TextMeshPro>().text = (scoreValue * time).ToString();
       
        if (SceneManager.GetActiveScene().name != "Tuto")
            SoundDisplay.Instance.RemoveEnnemy(gameObject);
        else
            SoundDisplqyTuto.Instance.RemoveEnnemy(gameObject);
       
        Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

}
