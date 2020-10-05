using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundDisplay : Singleton<SoundDisplay>
{
    public float beat;
    private float beatForBigAttack;
    double displayTime;
    double timePreviousBeat;
    [Range(0,100)]
    public double pourcentageAllow;
    double pourcentageCalculated;
    double timer;
    public double bpm;
    [Range(1,3)]
    public int speedModifier = 1;
    private float speed ;

    public AudioSource[] loops = new AudioSource[2];
    public AudioSource fail;

    public bool cantAct;
    [HideInInspector] public List<GameObject> ennemys = new List<GameObject>();
    [HideInInspector] public GameObject specialEnnemy;
    private AudioSource mainBeat;
    // Start is called before the first frame update
    void Start()
    {
        mainBeat = GetComponent<AudioSource>();
        timePreviousBeat = displayTime;
        bpm = Getbmp();
        pourcentageCalculated = pourcentageAllow / 100 * bpm;
    }

    // Update is called once per frame
    void Update()
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
        displayTime = mainBeat.time ;
        timer = displayTime - timePreviousBeat;
        Main.Instance.sprite.color = new Color(255, 0, 0, (float)( timer / bpm));

        
        if (displayTime >= timePreviousBeat + bpm)
        {
            beat ++;
            
            if(beat == 2 && !cantAct)
            {
            beatForBigAttack++;
               if (beatForBigAttack == 8)
                {
                    beatForBigAttack = 0;
                    Main.Instance.SpecialSpawn();

                }
                else
                {
                    if(specialEnnemy != null)
                    {
                         specialEnnemy.GetComponent<SpecialEnnemyBehavior>().Move();
                    // tu t'es arrete ici, faut faire le graph de l'onde, son anim a chaque beat, special attaakc a faire et deplacement du truc
                     }
                    for (int i = 0; i < ennemys.Count; i++)
                    {
                        ennemys[i].GetComponent<EnnemyBehavior>().Move();

                    }

                    Main.Instance.Spawn();

                }
                
                

                
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
			Main.Instance.canShoot = false;

		if (timer >= bpm - pourcentageCalculated
            //|| timer >= bpm / 2 - pourcentageAllow / 100 * bpm && timer <= bpm / 2 + pourcentageAllow / 100 * bpm
            )
		{
            Main.Instance.canShoot = true;

		}
    }
    public double Getbmp()
    {
        if (bpm > 10)
            return  60/bpm;
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
