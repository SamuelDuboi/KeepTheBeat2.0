using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "NewPattern", menuName = "Pattern", order = 12)]
public class Patterns : ScriptableObject
{
    public List<Vector3> ennemiesPositionForEditor;
    public List<Vector3> ennemiesPosition;
    public int difficulty;
    public void Initialize()
    {
       
        if (ennemiesPositionForEditor == null || ennemiesPositionForEditor.Count == 0)
        {
            //Initialize the list
            ennemiesPositionForEditor = new List<Vector3>();

        }
        if (ennemiesPosition == null || ennemiesPosition.Count == 0)
        {
            //Initialize the list
            ennemiesPosition = new List<Vector3>();

        }

    }

    public void AddEnnemy(Vector3 position, int i, int x)
    {
        ennemiesPositionForEditor.Add(position);
        ennemiesPosition.Add(new Vector3(i,x,position.z));
    }
    public void RemoveEnnemy(Vector3 position, int i, int x)
    {
        if (ennemiesPositionForEditor.Contains(position))
        {
            ennemiesPositionForEditor.Remove(position);
            ennemiesPosition.Remove(new Vector3(i,x,position.z));

        }
        else
            Debug.Log(position);
    }
}
