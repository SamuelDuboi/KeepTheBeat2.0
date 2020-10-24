using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( fileName ="New LeaderBoard", menuName ="LeaderBoard")]
[System.Serializable]
public class LeaderBoard : ScriptableObject
{
    public float[] scores = new float[10] ;

    public void CountScore(int value)
    {        
        for (int i = 0; i < scores.Length; i++)
        {
            if(value < scores[i] )
            {
                // if the player is to bad, dont put it in this glorious leader board. We dont like filthy pawns.
                if (i == 0)
                    break;
                else
                {
                    scores[0] = scores[1];
                    for (int x = 1; x < i-1; x++)
                    {
                        scores[x] = scores[x + 1];
                    }
                    scores[i - 1] = value;
                    break;
                }
            }
            //if the player is the best of the world;
            if(i == scores.Length - 1)
            {
                scores[0] = scores[1];
                for (int x = 1; x < i - 1; x++)
                {
                    scores[x] = scores[x + 1];
                }
                scores[i] = value;
            }
        }
    }
}
