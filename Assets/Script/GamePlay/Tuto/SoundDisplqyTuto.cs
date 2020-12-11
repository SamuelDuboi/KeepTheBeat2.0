using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AudioHelm;
using System;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
public class SoundDisplqyTuto : Singleton<SoundDisplqyTuto>
{

    public float beat;
    private double timePreviousBeat;
    [Range(0, 100)]
    public float pourcentageAllow;
    private double pourcentageCalculated;
    [SerializeField] private double timer;
    public AudioMixer mixer;
    [Range(1, 3)]
    public int speedModifier = 1;

    private bool doOnceBeat;
    public AudioSource[] loops = new AudioSource[2];
    public AudioSource fail;

    public bool cantAct;
    public bool canStart;
    public List<GameObject> ennemys = new List<GameObject>();

    public int cptForMovement;
    [HideInInspector] public bool doOnce;

    [Header("ObjectToScale")]
    public GameObject heart;
    public Image bpmVisuelG;
    public Image bpmVisuelD;

    [HideInInspector] public bool isBoss;
    private float timerBossPuls;
    private bool goNext;
    private float timerEnd;
    protected override void Awake()
    {
        base.Awake();
    }
    // Start is called before the first frame update
    void Start()
    {
        pourcentageCalculated = pourcentageAllow / 100 * (60 / AudioHelmClock.Instance.bpm);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        pourcentageCalculated = pourcentageAllow / 100f * (60f / AudioHelmClock.Instance.bpm);

        if (ennemys.Count == 0 && cantMove )
        {
            cantMove = false;
            currentEnnemyMovment = 0;
            MainTuto.Instance.cantSpwan = false;
            text.ClearText();
            

        }
        if (ennemys.Count == 0 && MainTuto.Instance.cptPhase > 16 && !goNext)
        {
            text.NextText(text.texts.Length - 1);
            timerEnd += Time.deltaTime;
            if(timerEnd >=1f)
            goNext = true;
        }
        else if (ennemys.Count == 0)
        {
            MainTuto.Instance.cantSpwan = false;
            currentEnnemyMovment = 0;
        }

        if (goNext && Input.anyKeyDown)
            SceneManager.LoadScene("Main");

        if (!isBoss)
            timer = AudioSettings.dspTime - AudioHelmClock.Instance.startTime - timePreviousBeat;
        else
        {
            timerBossPuls += Time.deltaTime;
            timer = timerBossPuls * 5;
            if (timerBossPuls > 0.1f)
            {
                timerBossPuls = 0;

            }
        }
        //MainTuto.Instance.sprite.color = new Color(255, 0, 0, (float)(timer / (60 / clock.bpm)));
        
            ScaleHeart();
            ScaleUI();
        
    }
    public bool cantMove;

    private int currentEnnemyMovment;

    public TutoText text;
    public void MoveEnnemy()
    {        
        if(currentEnnemyMovment >= 3 && ennemys.Count != 0 && MainTuto.Instance.cptPhase == 1 
            || currentEnnemyMovment >= 3 && ennemys.Count != 0 && MainTuto.Instance.cptPhase == 5 
            || currentEnnemyMovment >= 3 && ennemys.Count != 0 && MainTuto.Instance.cptPhase == 9 
            || currentEnnemyMovment >= 3 && ennemys.Count != 0 && MainTuto.Instance.cptPhase == 13 
            || currentEnnemyMovment >= 5 && ennemys.Count != 0 && MainTuto.Instance.cptPhase == 17)
        {
            
            if(MainTuto.Instance.cptPhase == 17)
            {
                MainTuto.Instance.canShootSpecial = true;
                MainTuto.Instance.canShoot = false;

            }
            MainTuto.Instance.cptPhase++;
            cantMove = true;
        }
        if(ennemys.Count == 0 && MainTuto.Instance.cptPhase%2 !=0)
        {
            MainTuto.Instance.cptPhase++;

        }
        currentEnnemyMovment++;
        timePreviousBeat = AudioSettings.dspTime - AudioHelmClock.Instance.startTime;
        if (!cantMove)
        {
            for (int i = 0; i < ennemys.Count; i++)
            {
                if (ennemys.Count > 0)
                {
                    if (ennemys[i].tag == "LinkedEnnemy")
                    {
                        foreach (var item in ennemys[i].GetComponent<LinkedEnnemy>().hitBox)
                        {
                            item.GetComponent<EnnemyBehavior>().Move();
                        }
                    }
                    else
                        ennemys[i].GetComponent<EnnemyBehavior>().Move();

                }
            }
            MainTuto.Instance.Spawn();
        }
    }


    public void AddEnnemy(GameObject ennemy)
    {
        if (ennemys == null)
            ennemys = new List<GameObject>();
        ennemys.Add(ennemy);
    }
    public void RemoveEnnemy(GameObject ennemy)
    {
        if (ennemy.GetComponent<EnnemyBehavior>() != null && ennemy.GetComponent<EnnemyBehavior>().tile != null)
            ennemy.GetComponent<EnnemyBehavior>().tile.GetComponent<TilesBehavior>().Off();
        
        ParticuleManager.Instance.ActivateEffects();
        ennemys.Remove(ennemy);
    }

    public void Unmute(int loopNUmber)
    {
        if (loopNUmber >= 0 && loopNUmber < loops.Length)
            loops[loopNUmber].mute = false;
    }
    public void TakeDamage(int loopNUmber)
    {
        
        //j'ai change ca c'était ennemy count -1 avant je sais pas pourquoi si ca bug c'est ici
        int _ennemyNumber = ennemys.Count;
        fail.Play();
        for (int i = 0; i < _ennemyNumber; i++)
        {
            if (ennemys[0].tag == "LinkedEnnemy")
            {
                ennemys[0].GetComponent<LinkedEnnemy>().DestroyAll(false);
            }
            else
            {
                if (ennemys[0].GetComponent<EnnemyBehavior>().tile != null)
                    ennemys[0].GetComponent<EnnemyBehavior>().tile.GetComponent<TilesBehavior>().Off();

                Destroy(ennemys[0]);
                ennemys.RemoveAt(0);

            }
        }
        if(MainTuto.Instance.cptPhase %2 !=0)
        MainTuto.Instance.cptPhase--;
        currentEnnemyMovment = 0;
        MainTuto.Instance.waveNumber = -0;
    }

    public void ScaleHeart()
    {
        // 80 = base Scale
        // BPM = [0;1]
        // MAX = 100
        // 80=> 100;
        float scaleFactor = 80 + (float)timer / (60 / AudioHelmClock.Instance.bpm) * 20;
        heart.transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
    }

    public void ScaleObject(GameObject obj, float scale, float mult)
    {
        float scaleFactor = scale + (float)timer / (60 / AudioHelmClock.Instance.bpm) * mult;
        obj.transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
    }

    public void ScaleUI()
    {
        float intervale = MainTuto.Instance.specialMaxValue - MainTuto.Instance.specialCount;

        float scaleFactorVisuel = (float)timer / (60 / AudioHelmClock.Instance.bpm) / 2;

        bpmVisuelG.fillAmount = 1 - scaleFactorVisuel - intervale / 100 - 0.1f;

        bpmVisuelD.fillAmount = bpmVisuelG.fillAmount;

        //Debug.Log(scaleFactorVisuel);
    }

    
}


