using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Score : Singleton<Score>
{
    public static float score;
    public LeaderBoard leaderBoard;
    [HideInInspector] public int scorMultiplier = 1;
    [HideInInspector] public int cptStreak;
    public TextMeshProUGUI scorText;
    public TextMeshProUGUI scorMultiplierText;
    private Animator animator;

    [SerializeField] GameObject UiEffects;

    public GameObject endSceneCanvas;
    public TextMeshProUGUI endState;
    public GameObject explosion;
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
        

        if (cptStreak >= 6)
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
        StartCoroutine(WaitEnd(victory));
    }
    
    IEnumerator WaitEnd(bool victory)
    {
        SoundDisplay.Instance.cantAct = true;
        SoundDisplay.Instance.cantMove = true;
        SoundDisplay.Instance.canStart = false;
        if (!victory)
        {
            GameObject DeadSound = Instantiate(Main.Instance.deathBossSound, transform.position, Quaternion.identity);
            Destroy(DeadSound, 8f);
            explosion.SetActive(true);
            explosion.GetComponentInParent<MeshRenderer>().enabled = false;
            yield return new WaitForSeconds(1f);
        }
        else
        {
            Main.Instance.positionEnd[2].GetComponent<Spawner>().DesaapppearAll();
            Main.Instance.positionEnd[3].GetComponent<Spawner>().DesaapppearAll();
        }
        PostProcessManager.post.ActivatePostProcess((int)postProcess.EndOfGame);
        endSceneCanvas.SetActive(true);
        leaderBoard.CountScore(score, AudioHelm.AudioHelmClock.Instance.bpm, victory);


        if (victory)
        {
            endState.text = "victory";
        }
        else
        {
            endState.text = "game over";
        }
        yield return new WaitForSeconds(10f);
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
