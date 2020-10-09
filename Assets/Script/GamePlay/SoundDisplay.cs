using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundDisplay : Singleton<SoundDisplay>
{
    public float beat;
    private double displayTime;
    private float pcmInSeconds;
    private double timePreviousBeat;
    private double loopNumber;
    [Range(0,100)]
    public double pourcentageAllow;
    private  double pourcentageCalculated;
    private  double timer;
    public double bpm;
    [Range(1,3)]
    public int speedModifier = 1;
    private float speed ;

    public AudioSource[] loops = new AudioSource[2];
    public AudioSource fail;

    public bool cantAct;
    [HideInInspector] public List<GameObject> ennemys = new List<GameObject>();
    private AudioSource mainBeat;

    private bool doOnce;
    private int doOnceCPT;
    // Start is called before the first frame update
    void Start()
    {
        mainBeat = GetComponent<AudioSource>();
        timePreviousBeat = displayTime;
        bpm = Getbmp();
        pourcentageCalculated = pourcentageAllow / 100 * bpm;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        switch (speedModifier)
        {
            case 0:
                speed = 0;
                break;
            case 1:
                speed = 1;
                break;

            case 2:
                speed = 1.5f;
                break;
            case 3:
                speed = 2f;
                break;
            default:
                speed = 1;
                break;
        }
        mainBeat.pitch = speed;
        pcmInSeconds = mainBeat.timeSamples / (mainBeat.pitch * 1000000) * 22;//frequence of 22 khz per pitch, convert PCM to sec

        /*if (Time.time >= mainBeat.clip.length+ loopNumber)
        {
            loopNumber += mainBeat.clip.length;
        }
        if (!mainBeat.isPlaying)
        {
            loopNumber += mainBeat.clip.length;
            mainBeat.Play();
        }*/
            displayTime = loopNumber +  pcmInSeconds; 

        timer = displayTime - timePreviousBeat;

        
        if (displayTime >= timePreviousBeat + bpm)
        {
            beat ++;
            
            if (beat == 2 && !cantAct)
            {

              /*  for (int i = 0; i < ennemys.Count; i++)
                {
                    ennemys[i].GetComponent<EnnemyBehavior>().Move();

                }

                Main.Instance.Spawn();
                */
                beat = 0;
            }
            else if(cantAct && beat >=3)
            {
                cantAct = false;
                beat = 0;
            }
            timePreviousBeat = displayTime;

        }
        CanAttack();
    }

   
    void CanAttack()
    {
        Main.Instance.sprite.color = new Color(255, 0, 0, (float)(timer / bpm));

        if (timer >= pourcentageCalculated
            && timer <= bpm-pourcentageCalculated
            //||timer <=bpm- pourcentageAllow / 100 * bpm && timer >= bpm / 2 + pourcentageAllow / 100 * bpm
            )
        {
            doOnce = true;
			Main.Instance.canShoot = false;
            doOnceCPT = 0;
        }

		if (timer >= bpm - pourcentageCalculated
            //|| timer >= bpm / 2 - pourcentageAllow / 100 * bpm && timer <= bpm / 2 + pourcentageAllow / 100 * bpm
            )
		{
            if (doOnce)
            {
                Main.Instance.canShoot = true;
                if(doOnceCPT < 2)
                {
                doOnce = false;
                    doOnceCPT++;
                }
            }
            
		}
    }
    public double Getbmp()
    {
        if (bpm > 10)
            return  60/(bpm+7);
        else
            return bpm;
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
        LifeManager.Instance.TakeDamage(loops, mainBeat);
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
