﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Score : Singleton<Score>
{
    public static int score;
    public LeaderBoard leaderBoard;
    [HideInInspector] public int scorMultiplier =1;
    [HideInInspector] public int cptStreak;
    public TextMeshProUGUI scorText;
    public TextMeshProUGUI scorMultiplierText;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   public void ScoreUp(int upNumber)
   {
        scorText.GetComponent<Animation>().Play();
        score += upNumber*scorMultiplier;
        cptStreak++;
        
        scorText.text =  score.ToString();
        if(cptStreak >= 6)
        {
            SoundDisplay.Instance.Unmute(scorMultiplier-1);
            if(scorMultiplier<8)
            scorMultiplier++;
            scorMultiplierText.text = "X" + scorMultiplier.ToString();
            cptStreak = 0;
            SoundManager.Instance.UpdateVolume(scorMultiplier);
        }
   }

    public void ScoreDown(int upNumber)
    {
        score -= upNumber ;
        if(cptStreak!=0)
            cptStreak--;
        scorText.text = score.ToString();
        
    }

    public void ModifierDown()
    {
        cptStreak = 0;
        if (scorMultiplier > 1)
        {
            scorMultiplier --;
            SoundManager.Instance.UpdateVolume(scorMultiplier);
            scorMultiplierText.text = "X" + scorMultiplier.ToString();
        }
        SoundDisplay.Instance.TakeDamage(scorMultiplier-1);

    }

    public void EndScene(bool victory)
    {
        leaderBoard.CountScore(score, victory);
        
        SceneManager.LoadScene("LeaderBoard");
    }
}
