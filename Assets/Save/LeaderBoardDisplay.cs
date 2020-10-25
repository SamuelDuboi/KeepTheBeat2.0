using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LeaderBoardDisplay : MonoBehaviour
{
    public LeaderBoard leaderBoard;
    private TextMeshProUGUI scores;
    public TMP_InputField namesInput;
    void Start()
    {
        scores = GetComponent<TextMeshProUGUI>();
        for (int i = 0; i < leaderBoard.scores.Length; i++)
        {
            scores.text += i+1.ToString() + "  " + leaderBoard.scores[i].ToString() + "\n";
        }
        if (leaderBoard.placeInLeaderBoard < 1000)
        {
           // namesInput.gameObject.SetActive(true);
        }
    }

    private void Update()
    {
        if (leaderBoard.placeInLeaderBoard < 1000)
        {

        }
    }

}
