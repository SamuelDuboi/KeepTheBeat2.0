using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Score : Singleton<Score>
{
    public static int score;
    /*[HideInInspector]*/ public float scorMultiplier = 1;
    /*[HideInInspector]*/ public int cptStreak;
    public TextMeshProUGUI scorText;
    public TextMeshProUGUI scorMultiplierText;
    private Animator animator;

    [SerializeField] GameObject UiEffects;

    public GameObject endSceneCanvas;
    public TextMeshProUGUI endState;
    public GameObject explosion;
    private bool oneEnd;
    protected override void Awake()
    {
        base.Awake();
    }
    private void Start()
    {
        animator = GetComponent<Animator>();
    }


    public void ScoreUp(int upNumber)
    {
       
        if(scorMultiplier < 7) //augmente cptstreak
        {
            cptStreak++;
            UiEffects.GetComponent<UIEffects>().Reset();
            UiEffects.GetComponent<UIEffects>().UpdateVisuel();
            animator.SetTrigger("Score");
            StartCoroutine("DelayScore", upNumber);

            if(cptStreak >= 6) // Si cptStreak suffisant.
            {
                MultiplicateurUp();
            }

        }

    }

    public void MultiplicateurUp()
    {
      
            cptStreak = 0;
            UiEffects.GetComponent<UIEffects>().Reset();
            scorMultiplierText.GetComponent<Animator>().SetTrigger("Multiplicateur");
            scorMultiplier++;
            scorMultiplierText.text = "X" + scorMultiplier.ToString();
            SoundManager.Instance.UpdateVolume(scorMultiplier);

    }

    public void ScoreDown(int upNumber)
    {
        if (score - upNumber <= 0)
            score = 0;
        else
            score -= upNumber ;
        if(cptStreak!=0)
            cptStreak--;
        if(scorText)
        scorText.text = score.ToString();
        UiEffects.GetComponent<UIEffects>().UpdateVisuel();

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
        UiEffects.GetComponent<UIEffects>().UpdateVisuel();

    }

    public void EndScene(bool victory)
    {
        if (!oneEnd)
        {
            StartCoroutine(WaitEnd(victory));
            oneEnd = true;
        }
    }
    
    IEnumerator WaitEnd(bool victory)
    {
       
       var _scen = SceneManager.LoadSceneAsync("LeaderBoard", LoadSceneMode.Single);
        _scen.allowSceneActivation = false;
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
        


        if (victory)
        {
            endState.text = "victory";
        }
        else
        {
            endState.text = "game over";
        }
        yield return new WaitForSeconds(6f);
        LeaderBoard.instance.CountScore(score, (int)AudioHelm.AudioHelmClock.Instance.bpm, victory);
        Destroy(AudioHelm.AudioHelmClock.Instance.gameObject);
        _scen.allowSceneActivation = true;

    }


    public void EndAnim()
    {
        animator.SetBool("Score", false);
    }

    IEnumerator DelayScore(float scoreToAdd)
    {
        for (int i = 0; i < 10; i++)
        {
            score +=  (int)(scoreToAdd * scorMultiplier / 10);
            scorText.text = score.ToString();
            yield return new WaitForSeconds(0.01f);
        }
        
    }
}
