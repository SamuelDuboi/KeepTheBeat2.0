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
    public double pourcentageAllow;
    private  double pourcentageCalculated;
    private  double timer;
    public AudioHelmClock clock;
    public AudioMixer mixer;
    [Range(1,3)]
    public int speedModifier = 1;

    public AudioSource[] loops = new AudioSource[2];
    public AudioSource fail;

    public bool cantAct;
    [HideInInspector] public List<GameObject> ennemys = new List<GameObject>();

   [HideInInspector] public bool doOnce;
    // Start is called before the first frame update
    void Start()
    {
        pourcentageCalculated = pourcentageAllow / 100 * (60/clock.bpm);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
       

        /*if (Time.time >= mainBeat.clip.length+ loopNumber)
        {
            loopNumber += mainBeat.clip.length;
        }
        if (!mainBeat.isPlaying)
        {
            loopNumber += mainBeat.clip.length;
            mainBeat.Play();
        }*/


        timer = AudioHelmClock.GetGlobalBeatTime() - timePreviousBeat;
        CanAttack();
    }

    public void BeatEvent()
    {
        beat++;
        if(beat == 4 )
        timePreviousBeat = AudioHelmClock.GetGlobalBeatTime();

        if (beat == 8 && !cantAct)
        {
            timePreviousBeat = AudioHelmClock.GetGlobalBeatTime();

            for (int i = 0; i < ennemys.Count; i++)
            {
                ennemys[i].GetComponent<EnnemyBehavior>().Move();

            }

            Main.Instance.Spawn();

            beat = 0;
        }
        else if (cantAct && beat >= 9)
        {
            cantAct = false;
            beat = 0;
        }
        
        
    }
   
    void CanAttack()
    {
        Main.Instance.sprite.color = new Color(255, 0, 0, (float)(timer/(60/clock.bpm )));

        if (timer >= pourcentageCalculated
            && timer <= 60/clock.bpm-pourcentageCalculated
            //||timer <=bpm- pourcentageAllow / 100 * bpm && timer >= bpm / 2 + pourcentageAllow / 100 * bpm
            )
        {
            Main.Instance.CantShoot();
        }

		if (timer >= 60/clock.bpm - pourcentageCalculated
            //|| timer >= bpm / 2 - pourcentageAllow / 100 * bpm && timer <= bpm / 2 + pourcentageAllow / 100 * bpm
            )
		{
            Main.Instance.CanShoot();
            
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
            Destroy(ennemys[0]);
            ennemys.RemoveAt(0);
        }
        cantAct = true;
        if (loopNUmber >= 0 && loopNUmber < loops.Length)
            loops[loopNUmber].mute = true;
    }
}
