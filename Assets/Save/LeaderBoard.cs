using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( fileName ="New LeaderBoard", menuName ="LeaderBoard")]
[System.Serializable]
public class LeaderBoard : ScriptableObject
{
    public float[] scores = new float[10] ;
    public string[] names = new string[10] ;
    public int placeInLeaderBoard;

    public bool victory;
    public bool gameOver;
    public void CountScore(int value, bool isVictory)
    {
        victory = false;
        gameOver = false;
        if (isVictory)
            victory = true;
        else
            gameOver = true;
        placeInLeaderBoard = 1000;
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
                    names[0] = names[1];
                    for (int x = 1; x < i-1; x++)
                    {
                        scores[x] = scores[x + 1];
                        names[x] = names[x + 1];
                    }
                    placeInLeaderBoard = i - 1;
                    scores[i - 1] = value;
                    break;
                }
            }
            if (value == scores[i])
            {
                // if the player is to bad, dont put it in this glorious leader board. We dont like filthy pawns.
                if (i == 0)
                    break;
                else if (i == 1)
                    break;
                else
                {
                    scores[0] = scores[1];
                    names[0] = names[1];
                    for (int x = 1; x < i - 1; x++)
                    {
                        scores[x] = scores[x ];
                        names[x] = names[x];
                    }
                    placeInLeaderBoard = i - 2;
                    scores[i - 2] = value;
                    break;
                }
            }
            //if the player is the best of the world;
            if (i == scores.Length - 1)
            {
                scores[0] = scores[1];
                names[0] = names[1];
                for (int x = 1; x < i - 1; x++)
                {
                    scores[x] = scores[x + 1];
                    names[x] = names[x + 1];

                }
                placeInLeaderBoard = i;
                scores[i] = value;
            }
        }
    }
}
