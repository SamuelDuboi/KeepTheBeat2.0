using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : Singleton<Score>
{
    public static int score;
   
   [HideInInspector] public int scorMultiplier =1;
    [HideInInspector] public int cptStreak;
    public Text scorText;
    public Text scorMultiplierText;
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
        score += upNumber*scorMultiplier;
        cptStreak++;
        scorText.text = "Score : " + score.ToString();
        if(cptStreak >= 2)
        {
            SoundDisplay.Instance.Unmute(scorMultiplier-1);
            scorMultiplier++;
            scorMultiplierText.text = "X" + scorMultiplier.ToString();
            cptStreak = 0;
        }
    }

    public void ModifierDown()
    {
        cptStreak = 0;
        if (scorMultiplier > 1)
        {
            scorMultiplier --;
            scorMultiplierText.text = "X" + scorMultiplier.ToString();
        }
        SoundDisplay.Instance.TakeDamage(scorMultiplier-1);

    }
}
