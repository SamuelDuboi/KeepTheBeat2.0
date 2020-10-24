using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LeaderBoardDisplay : MonoBehaviour
{
    public LeaderBoard leaderBoard;
    private TextMeshProUGUI scores;
    void Start()
    {
        scores = GetComponent<TextMeshProUGUI>();
        for (int i = 0; i < leaderBoard.scores.Length; i++)
        {
            scores.text += i+1.ToString() + "  " + leaderBoard.scores[i].ToString() + "\n";
        }
    }

}
