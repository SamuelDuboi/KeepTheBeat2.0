using AudioHelm;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SoundDisplay : Singleton<SoundDisplay>
{

    public float beat;
   [HideInInspector] public double timePreviousBeat;
    [Range(0, 100)]
    public float pourcentageAllow;
    private double pourcentageCalculated;
    [SerializeField] private double timer;
    public AudioHelmClock clock;
    public AudioMixer mixer;
    [Range(1, 3)]
    public int speedModifier = 1;

    private bool doOnceBeat;
    public AudioSource fail;

    public bool cantAct;
     public List<GameObject> ennemys = new List<GameObject>();

    public int cptForMovement;
    [HideInInspector] public bool doOnce;

    [Header("ObjectToScale")]
    public GameObject heart;
    public Image bpmVisuelG;
    public Image bpmVisuelD;

    [HideInInspector] public bool isBoss;
    private float timerBossPuls;
    // Start is called before the first frame update
    void Start()
    {
        pourcentageCalculated = pourcentageAllow / 100 * (60 / clock.bpm);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        pourcentageCalculated = pourcentageAllow / 100f * (60f / clock.bpm);

        if(!isBoss)
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
        //Main.Instance.sprite.color = new Color(255, 0, 0, (float)(timer / (60 / clock.bpm)));
        if (!cantMove)
        {
            ScaleHeart();
            ScaleUI();
        }
        else
        {
            timePreviousBeat = AudioSettings.dspTime - AudioHelmClock.Instance.startTime;
        }
    }
    public bool cantMove;
    public void MoveEnnemy()
    {
        if (!cantMove)
        {
            timePreviousBeat = AudioSettings.dspTime - AudioHelmClock.Instance.startTime;
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
                Main.Instance.Spawn();
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
        if (ennemy.GetComponent<EnnemyBehavior>() != null&& ennemy.GetComponent<EnnemyBehavior>().tile != null)
            ennemy.GetComponent<EnnemyBehavior>().tile.GetComponent<TilesBehavior>().Off();
        ParticuleManager.Instance.ActivateEffects();
        ennemys.Remove(ennemy);
    }


    public void TakeDamage(int loopNUmber)
    {
        LifeManager.Instance.TakeDamage(mixer);
        //j'ai change ca c'était ennemy count -1 avant je sais pas pourquoi si ca bug c'est ici
        int _ennemyNumber = ennemys.Count;
        fail.Play();
        for (int i = 0; i < _ennemyNumber; i++)
        {
            if (ennemys[0].tag == "LinkedEnnemy")
            {
                ennemys[0].GetComponent<LinkedEnnemy>().DestroyAll();
            }
            else
            {
                if (ennemys[0].GetComponent<EnnemyBehavior>().tile != null)
                    ennemys[0].GetComponent<EnnemyBehavior>().tile.GetComponent<TilesBehavior>().Off();

                Destroy(ennemys[0]);
                ennemys.RemoveAt(0);

            }
        }
        cantAct = true;

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

    public void ScaleObject(GameObject obj,float scale,float mult)
    {
        float scaleFactor =scale + (float)timer / (60 / AudioHelmClock.Instance.bpm) * mult;
       obj.transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
    }

    public void ScaleUI()
    {
        float intervale = Main.Instance.specialMaxValue - Main.Instance.specialCount;

        float scaleFactorVisuel = (float)timer / (60 / AudioHelmClock.Instance.bpm) /2;
        
        bpmVisuelG.fillAmount = 1 - scaleFactorVisuel - intervale/100 - 0.1f;

        bpmVisuelD.fillAmount = bpmVisuelG.fillAmount;

        //Debug.Log(scaleFactorVisuel);
    }
}
