using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject pawn;
    public GameObject main;
    public GameObject ennemy;
    public int timeForSpawn;
    private double waitForSpawn;
    private Vector2 distanceToMove;
    [HideInInspector] public Vector2[] positions = new Vector2[6];
    private Color currentColor;
    // Start is called before the first frame update
    void Start()
    {
        currentColor = GetComponent<SpriteRenderer>().color;
        StartSpawn();
    }


    void StartSpawn()
    {
        distanceToMove.x = (main.transform.position.x - transform.position.x) / 6;
        distanceToMove.y = (main.transform.position.y - transform.position.y) / 6;
        for (int i = 0; i < 6; i++)
        {
            positions[i] = new Vector2(transform.position.x + distanceToMove.x * i, transform.position.y + distanceToMove.y * i);
            GameObject _pawn = Instantiate(pawn, positions[i], Quaternion.identity, transform);
            _pawn.GetComponent<SpriteRenderer>().color = currentColor;

        }

    }

    public void Spwan()
    {
        GameObject _ennemy = Instantiate(ennemy, transform);
        SoundDisplay.Instance.AddEnnemy(_ennemy);
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


