using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Score : Singleton<Score>
{
    public static int score;
    public LeaderBoard leaderBoard;
    [HideInInspector] public int scorMultiplier = 1;
    [HideInInspector] public int cptStreak;
    public TextMeshProUGUI scorText;
    public TextMeshProUGUI scorMultiplierText;


    public void ScoreUp(int upNumber)
    {

        score += upNumber * scorMultiplier;
        cptStreak++;
        scorText.text = score.ToString();
        scorText.GetComponent<Animation>().Play();

        if (cptStreak >= 10)
        {
            if (scorMultiplier < 7)
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
            cptStreak = 0;
        scorText.text = score.ToString();

    }

    public void ModifierDown()
    {
        cptStreak = 0;
        if (scorMultiplier > 1)
        {
            scorMultiplier--;
            SoundManager.Instance.UpdateVolume(scorMultiplier);
            scorMultiplierText.text = "X" + scorMultiplier.ToString();
        }
        if (SceneManager.GetActiveScene().name != "Tuto")
            SoundDisplay.Instance.TakeDamage(scorMultiplier - 1);
        else
            SoundDisplqyTuto.Instance.TakeDamage(scorMultiplier - 1);

    }

    public void EndScene(bool victory)
    {
        leaderBoard.CountScore(score, victory);

        SceneManager.LoadScene("LeaderBoard");
    }


}
