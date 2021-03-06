﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "NewPattern", menuName = "Pattern", order = 12)]
public class Patterns : ScriptableObject
{
    public List<Vector2> ennemiesPositionForEditor;
    public List<Vector2> ennemiesPosition;
    public int difficulty;
    public void Initialize()
    {
       
        if (ennemiesPositionForEditor == null || ennemiesPositionForEditor.Count == 0)
        {
            //Initialize the list
            ennemiesPositionForEditor = new List<Vector2>();

        }
        if (ennemiesPosition == null || ennemiesPosition.Count == 0)
        {
            //Initialize the list
            ennemiesPosition = new List<Vector2>();

        }

    }

    public void AddEnnemy(Vector2 position, int i, int x)
    {
        ennemiesPositionForEditor.Add(position);
        ennemiesPosition.Add(new Vector2(i,x));
    }
    public void RemoveEnnemy(Vector2 position, int i, int x)
    {
        if (ennemiesPositionForEditor.Contains(position))
        {
            ennemiesPositionForEditor.Remove(position);
            ennemiesPosition.Remove(new Vector2(i,x));

        }
        else
            Debug.Log("ya un problème");
    }
}
