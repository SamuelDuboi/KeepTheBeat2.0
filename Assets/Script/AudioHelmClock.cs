
using UnityEngine.SceneManagement;
using UnityEngine;

namespace AudioHelm
{

    public class AudioHelmClock : Singleton<AudioHelmClock>
    {

        public float bpm;
        private double timer;
        private double loopTimer;
        private bool doOnce;
        [HideInInspector] public double startTime;
        private void Start()
        {
            startTime = AudioSettings.dspTime;
        }
        private void FixedUpdate()
        {

            timer = AudioSettings.dspTime - startTime - loopTimer;
            if (SceneManager.GetActiveScene().name != "Intro")
            {
                if (timer <= (60 / bpm) / 4)
                {
                    //On Peut
                    if (SceneManager.GetActiveScene().name == "Main")

                        Main.Instance.CanShoot();
                    else if (SceneManager.GetActiveScene().name == "Tuto")
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
                else if (timer >= (60 / bpm) * 3 / 4 && timer < 60 / bpm)
                {
                    //On peu
                    if (SceneManager.GetActiveScene().name != "Tuto")
                        Main.Instance.CanShoot();
                    else
                        MainTuto.Instance.CanShoot();
                }

                else if (timer >= 60 / bpm)
                {
                    loopTimer = AudioSettings.dspTime - startTime;
                    timer = AudioSettings.dspTime - startTime - loopTimer;
                    if (!doOnce)
                    {
                        doOnce = true;

                        SoundManager.Instance.StartDelay();

                    }

                    if (SceneManager.GetActiveScene().name == "Main")
                        SoundDisplay.Instance.MoveEnnemy();
                    else if (SceneManager.GetActiveScene().name == "Tuto")
                        SoundDisplqyTuto.Instance.MoveEnnemy();

                }


            }
        }

    }
}
