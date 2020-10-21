﻿using System.Collections;
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

    public void Spwan(GameObject ennemy)
    {
        GameObject _ennemy = Instantiate(ennemy, transform);
        _ennemy.transform.position = new Vector3(transform.position.x,transform.position.y,transform.position.z * 100);
        SoundDisplay.Instance.AddEnnemy(_ennemy);

        _ennemy.GetComponentInChildren<Light>().color = currentColor;
        _ennemy.GetComponent<EnnemyBehavior>().positions = this.positions;
    }
    public void Spwan(GameObject ennemy, Vector3[] positions)
    {
        GameObject _ennemy = Instantiate(ennemy, transform);
        _ennemy.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z * 100);
        SoundDisplay.Instance.AddEnnemy(_ennemy);
        _ennemy.GetComponent<EnnemyBehavior>().turnOnTrail = true;
        _ennemy.GetComponentInChildren<Light>().color = currentColor;
        for (int i = 0; i < positions.Length; i++)
        {
            if(i%2 == 0)
            _ennemy.GetComponent<EnnemyBehavior>().positions[i] = this.positions[i];
            else
                _ennemy.GetComponent<EnnemyBehavior>().positions[i] = positions[i];


        }
    }
    public void Spwan(GameObject ennemy, Spawner firstSpawner, Spawner secondSpawner)
    {
        GameObject _ennemy = Instantiate(ennemy, transform);
        _ennemy.GetComponent<LinkedEnnemy>().hitBox[0].transform.position = new Vector3(firstSpawner.transform.position.x, firstSpawner.transform.position.y, firstSpawner.transform.position.z * 100);
        _ennemy.GetComponent<LinkedEnnemy>().hitBox[1].transform.position = new Vector3(secondSpawner.transform.position.x, secondSpawner.transform.position.y, secondSpawner.transform.position.z * 100);
        SoundDisplay.Instance.AddEnnemy(_ennemy);
        _ennemy.GetComponent<LinkedEnnemy>().hitBox[1].GetComponent<EnnemyBehavior>().turnOnTrail = true;
        _ennemy.GetComponent<LinkedEnnemy>().hitBox[0].GetComponent<EnnemyBehavior>().turnOnTrail = true;
        _ennemy.GetComponent<LinkedEnnemy>().hitBox[0].GetComponentInChildren<Light>().color = firstSpawner.currentColor;
        _ennemy.GetComponent<LinkedEnnemy>().hitBox[1].GetComponentInChildren<Light>().color = secondSpawner.currentColor;
        _ennemy.GetComponent<LinkedEnnemy>().hitBox[0].GetComponentInChildren<EnnemyBehavior>().positions = firstSpawner.positions;
        _ennemy.GetComponent<LinkedEnnemy>().hitBox[1].GetComponentInChildren<EnnemyBehavior>().positions = secondSpawner.positions;
      
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


