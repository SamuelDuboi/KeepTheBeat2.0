using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Score : Singleton<Score>
{
    public static int score;
    public LeaderBoard leaderBoard;
    [HideInInspector] public int scorMultiplier = 1;
    [HideInInspector] public int cptStreak;
    public TextMeshProUGUI scorText;
    public TextMeshProUGUI scorMultiplierText;
    private Animator animator;

    [SerializeField] GameObject UiEffects;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }


    public void ScoreUp(int upNumber)
    {
        if(scorMultiplier < 7)
        {
            cptStreak++;
        }

        animator.SetTrigger("Score");
        StartCoroutine("DelayScore", upNumber);
        

        if (cptStreak >= 5)
        {
            if (scorMultiplier < 7)
                scorMultiplierText.GetComponent<Animator>().SetTrigger("Multiplicateur");

            scorMultiplier++;
            scorMultiplierText.text = "X" + scorMultiplier.ToString();
            cptStreak = 0;
            UiEffects.GetComponent<UIEffects>().Reset();
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
        leaderBoard.CountScore(score,AudioHelm.AudioHelmClock.Instance.bpm, victory);
        if(victory)
            StartCoroutine(FadeEnd());
        else
        {
            SoundDisplay.Instance.canStart = false;
            SoundDisplay.Instance.cantAct = true;
            SoundDisplay.Instance.cantMove = true;
            SceneManager.LoadScene("LeaderBoard");

        }
    }
    
    IEnumerator FadeEnd()
    {
        yield return new WaitForSeconds(5f);
            SoundDisplay.Instance.cantAct = true;
            SoundDisplay.Instance.cantMove = true;
        SoundDisplay.Instance.canStart = false;
        SceneManager.LoadScene("LeaderBoard");
    }
   
    
    public void EndAnim()
    {
        animator.SetBool("Score", false);
    }

    IEnumerator DelayScore(int scoreToAdd)
    {
        for (int i = 0; i < 6; i++)
        {
            score += (scoreToAdd * scorMultiplier) / 6;
            scorText.text = score.ToString();
            yield return new WaitForSeconds(0.01f);
        }
        
    }
}
