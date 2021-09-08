
using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;
using TMPro;
namespace AudioHelm
{

    public class AudioHelmClock : Singleton<AudioHelmClock>
    {

        public double bpm;
        public double timer;
        private double loopTimer;
        private double timerMissed;
        public bool doOnce;
        public double timeToReach;
        
        [HideInInspector] public double startTime;

        protected override void Awake()
        {
            base.Awake();
        }
        private IEnumerator Start()
        {
            yield return new WaitForEndOfFrame();
            startTime = AudioSettings.dspTime;
            timerMissed = 0;
        }
        private void Update()
        {
            
            if(startTime != 0)
                timer = AudioSettings.dspTime - startTime - loopTimer;

            if (timer > 1.2)
            {
                loopTimer = AudioSettings.dspTime - startTime;
                timer = AudioSettings.dspTime - startTime - loopTimer;
            }
            if (SceneManager.GetActiveScene().name != "Intro")
            {
                if (timer <= (60 / bpm) / 4)
                {
                    //On Peut
                    if (SceneManager.GetActiveScene().name == "Main" && !Main.Instance.canShoot)
                    {
                        Main.Instance.CanShoot();
                    }

                    else if (SceneManager.GetActiveScene().name == "Tuto" && !MainTuto.Instance.canShoot)
                        MainTuto.Instance.CanShoot();
                }
                else if (timer >= (60 / bpm) / 3 && timer <= (60 / bpm) * 2 / 3)
                {
                    //On peut pas
                    if (SceneManager.GetActiveScene().name != "Tuto")
                        Main.Instance.CantShoot();
                    else
                        MainTuto.Instance.CantShoot();

                }
                else if (timer >= (60 / bpm) * 2 /3 && timer < 60 / bpm )
                {
                    //On peu
                    if (SceneManager.GetActiveScene().name != "Tuto" && !Main.Instance.canShoot)
                    {
                        Main.Instance.CanShoot();
                    }
                    else if (SceneManager.GetActiveScene().name == "Tuto" && !MainTuto.Instance.canShoot)
                        MainTuto.Instance.CanShoot();
                }
                if ( timer+ timerMissed >= 60 / bpm)
                {
                    //Debug.Log("timer " +timer);
                    //Debug.Log("timer missed" + timerMissed);
                    loopTimer = AudioSettings.dspTime -  startTime; 
                    if (!doOnce)
                    {
                        doOnce = true;
                        SoundManager.Instance.StartDelay();

                    }
                    timerMissed = timer +timerMissed- (60 / bpm);
                    if (Mathf.Abs((float)timerMissed )> 0.3)
                        timerMissed = 0;
                    timer = AudioSettings.dspTime - startTime - loopTimer;
                  //  Debug.Log("second "+timer);


                    if (SceneManager.GetActiveScene().name == "Main" && SoundDisplay.Instance.canStart)
                        SoundDisplay.Instance.MoveEnnemy();
                    else if (SceneManager.GetActiveScene().name == "Tuto" && SoundDisplqyTuto.Instance.canStart)
                        SoundDisplqyTuto.Instance.MoveEnnemy();

                }


            }
        }

    }
}
