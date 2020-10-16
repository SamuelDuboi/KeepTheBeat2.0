using AudioHelm;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundDisplay : Singleton<SoundDisplay>
{
    public float beat;
    private double timePreviousBeat;
    [Range(0,100)]
    public float pourcentageAllow;
    private  double pourcentageCalculated;
    private  double timer;
    public AudioHelmClock clock;
    public AudioMixer mixer;
    [Range(1,3)]
    public int speedModifier = 1;

    private bool doOnceBeat;
    public AudioSource[] loops = new AudioSource[2];
    public AudioSource fail;

    public bool cantAct;
    [HideInInspector] public List<GameObject> ennemys = new List<GameObject>();

    private int cptForMovement;
   [HideInInspector] public bool doOnce;
    // Start is called before the first frame update
    void Start()
    {
        pourcentageCalculated = pourcentageAllow / 100 * (60/clock.bpm);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        pourcentageCalculated = pourcentageAllow / 100f * (60f / clock.bpm);
        
        timer = AudioHelmClock.GetGlobalBeatTime() - timePreviousBeat;

        //Main.Instance.sprite.color = new Color(255, 0, 0, (float)(timer / (60 / clock.bpm)));
    }

    public void BeatEvent()
    {
        if (AudioHelmClock.GetGlobalBeatTime() < 0)
            return;
        beat++;

            if (beat == 0)
            {
                /* if (beat == 8)
                 {
                     timePreviousBeat = AudioHelmClock.GetGlobalBeatTime();
                 }*/
                
            }
            else if (beat == 8)
            {
                Main.Instance.CantShoot();
            }
            else if (beat == 12)
            {
                cptForMovement++;

            beat = 0;
            Main.Instance.CanShoot();
        }
            if (cptForMovement == 1 && beat == 4)
            {
                timePreviousBeat = AudioHelmClock.GetGlobalBeatTime();
                cptForMovement = 0;
                for (int i = 0; i < ennemys.Count; i++)
                {
                    ennemys[i].GetComponent<EnnemyBehavior>().Move();

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
        if (ennemy.GetComponent<EnnemyBehavior>().tile != null)
            ennemy.GetComponent<EnnemyBehavior>().tile.GetComponent<TilesBehavior>().Off();        
        ennemys.Remove(ennemy);
    }

    public void Unmute(int loopNUmber)
    {
        if(loopNUmber>=0 && loopNUmber<loops.Length)
        loops[loopNUmber].mute = false;
    }
    public void TakeDamage(int loopNUmber)
    {
        LifeManager.Instance.TakeDamage(mixer);
        int _ennemyNumber = ennemys.Count - 1;
        fail.Play();
        for (int i = 0; i < _ennemyNumber; i++)
        {
            if (ennemys[0].GetComponent<EnnemyBehavior>().tile != null)
                ennemys[0].GetComponent<EnnemyBehavior>().tile.GetComponent<TilesBehavior>().Off();
            Destroy(ennemys[0]);
            ennemys.RemoveAt(0);
        }
        cantAct = true;
        if (loopNUmber >= 0 && loopNUmber < loops.Length)
            loops[loopNUmber].mute = true;
    }
}
