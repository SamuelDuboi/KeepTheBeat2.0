using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject pawn;
    public GameObject direction;
    public GameObject ennemy;
    public int timeForSpawn;
    private double waitForSpawn;
    private Vector3 distanceToMove;
    public Vector3[] positions = new Vector3[6];
    private Color currentColor;
    // Start is called before the first frame update
    void Start()
    {
        currentColor = GetComponent<SpriteRenderer>().color;
        StartSpawn();
    }


    void StartSpawn()
    {
        distanceToMove.x = (direction.transform.position.x - transform.position.x) / 6;
        distanceToMove.y = (direction.transform.position.y - transform.position.y) / 6;
        distanceToMove.z = (direction.transform.position.z - transform.position.z) / 6;

        for (int i = 0; i < 6; i++)
        {
            positions[i] = new Vector3(transform.position.x + distanceToMove.x * i, transform.position.y + distanceToMove.y * i,transform.position.z + distanceToMove.z*i);
            GameObject _pawn = Instantiate(pawn, positions[i], Quaternion.identity, transform);


        }

    }

    public void Spwan()
    {
        GameObject _ennemy = Instantiate(ennemy, transform);
        _ennemy.transform.position = new Vector3(transform.position.x,transform.position.y,transform.position.z * 100);
        SoundDisplay.Instance.AddEnnemy(_ennemy);

        _ennemy.GetComponentInChildren<Light>().color = currentColor;
        _ennemy.GetComponent<EnnemyBehavior>().positions = this.positions;
    }
    /// <summary>
    /// Special spawner
    /// </summary>
    /// <param name="isSPecial"></param>
    public void Spwan(bool isSPecial)
    {
        GameObject _ennemy = Instantiate(ennemy, transform);
        if(isSPecial)
        {
            SoundDisplay.Instance.AddEnnemy(_ennemy);
            _ennemy.GetComponent<EnnemyBehavior>().positions = this.positions;

        }
    }

}


