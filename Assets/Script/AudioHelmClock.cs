
using UnityEngine.SceneManagement;
using UnityEngine;
using System;
namespace AudioHelm
{

    public class AudioHelmClock : Singleton<AudioHelmClock>
    {

        public double bpm;
        public double timer;
        private double loopTimer;
        private bool doOnce;
        public double timeToReach;
        [HideInInspector] public double startTime;
        protected override void Awake()
        {
            base.Awake();
        }
        private void Start()
        {
            startTime = AudioSettings.dspTime;
        }
        private void Update()
        {
           timer = AudioSettings.dspTime - startTime - loopTimer;

            if (timer > 1)
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
                else if (timer >= (60 / bpm) / 4 && timer <= (60 / bpm) * 3 / 4)
                {
                    //On peut pas
                    if (SceneManager.GetActiveScene().name != "Tuto")
                        Main.Instance.CantShoot();
                    else
                        MainTuto.Instance.CantShoot();

                }
                else if (timer >= (60 / bpm) * 3 / 4 && timer < 60 / bpm )
                {
                    //On peu
                    if (SceneManager.GetActiveScene().name != "Tuto" && !Main.Instance.canShoot)
                    {
                        Main.Instance.CanShoot();
                    }
                    else if (SceneManager.GetActiveScene().name == "Tuto" && !MainTuto.Instance.canShoot)
                        MainTuto.Instance.CanShoot();
                }
                if (Math.Round( timer*100) >=  timeToReach)
                {
                    Debug.Log(timer);
                    loopTimer = AudioSettings.dspTime -  startTime; 
                    if (!doOnce)
                    {
                        doOnce = true;
                        timeToReach = timer*100;
                        SoundManager.Instance.StartDelay();

                    }
                    timer = AudioSettings.dspTime - startTime - loopTimer;


                    if (SceneManager.GetActiveScene().name == "Main" && SoundDisplay.Instance.canStart)
                        SoundDisplay.Instance.MoveEnnemy();
                    else if (SceneManager.GetActiveScene().name == "Tuto" && SoundDisplqyTuto.Instance.canStart)
                        SoundDisplqyTuto.Instance.MoveEnnemy();

                }


            }
        }

    }
}
