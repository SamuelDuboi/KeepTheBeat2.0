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

        score += upNumber * scorMultiplier;
        scorText.text = score.ToString();
        animator.SetTrigger("Score");


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
        leaderBoard.CountScore(score, victory);

        SceneManager.LoadScene("LeaderBoard");
    }

    public void EndAnim()
    {
        animator.SetBool("Score", false);
    }

}
