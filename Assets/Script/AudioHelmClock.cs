// Copyright 2017 Matt Tytel

using UnityEngine;

namespace AudioHelm
{

    public class AudioHelmClock : Singleton<AudioHelmClock>
    {

        public float bpm;
        private double timer;
        private double loopTimer;
        private bool doOnce;
      [HideInInspector]  public double startTime;
        private void Start()
        {
            startTime = AudioSettings.dspTime;
        }
        private void FixedUpdate()
        {
            
           timer = AudioSettings.dspTime - startTime - loopTimer;
            if(timer <= (60 / bpm)/4 )
            {
                //On Peut
                Main.Instance.CanShoot();
            }
            else if(timer >= (60 / bpm) / 4 && timer<= (60/bpm)*3/4)
            {
                //On peut pas
                Main.Instance.CantShoot();

            }
            else if (timer >= (60 / bpm) * 3 / 4 && timer < 60/bpm )
            {
                //On peu
                Main.Instance.CanShoot();
            }           
            
           else if( timer>= 60 / bpm)
            {
                if (!doOnce)
                {
                    doOnce = true;
                    SoundManager.Instance.StartDelay();
                }
               SoundDisplay.Instance.MoveEnnemy();
              loopTimer = AudioSettings.dspTime - AudioHelmClock.Instance.startTime;
            }

        }

    }
}
