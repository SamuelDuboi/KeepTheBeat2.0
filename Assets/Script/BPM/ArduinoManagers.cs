using AudioHelm;
using System.Collections;
using System.Collections.Generic;
using Uduino;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Linq.Expressions;

public class ArduinoManagers : Singleton<ArduinoManagers>
{
    [Header("UI")]
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI bpm;
    public TextMeshProUGUI instructions;
    public GameObject circleRef;
    public GameObject heart;

    [Header("Values")]
    public float timerFloat;
    private List<int> numbers;
    private bool started;
    [SerializeField] private bool doOnce;
    private bool canCount;
    private double cpt;


    private bool loadOnce;

    public GameObject morpheus;

    public GameObject sound;

    public Image fade;

    [SerializeField] private GameObject SoundClic;
    protected override void Awake()
    {
        base.Awake();
    }
    private void Start()
    {
        timerFloat = 20;// to enable timer only when started = true
        UduinoManager.Instance.OnDataReceived += DataReceived;
        DontDestroyOnLoad(this);

    }

    private void Update()
    {
      
        if (SceneManager.GetActiveScene().name == "Main" || SceneManager.GetActiveScene().name == "Tuto")
        {           
            if(cpt < 65)
            {
                cpt = 60;
            }
            else if (cpt >= 65 && cpt < 70)
            {
                cpt = 65;
            } 
            else if (cpt >= 70 && cpt < 75)
            {
                cpt = 70;
            } 
            else if (cpt >= 75 && cpt < 80)
            {
                cpt = 75;
            } 
            else if (cpt >= 80 && cpt < 85)
            {
                cpt = 80;
                PostProcessManager.post.ActivatePostProcessInChild((int)postProcess.BPM_90);
            } 
            else if (cpt >= 85 && cpt < 90)
            {
                cpt = 85;
                PostProcessManager.post.ActivatePostProcessInChild((int)postProcess.BPM_90);
            } 
            else if (cpt >= 90 && cpt < 95)
            {
                cpt = 90;
                PostProcessManager.post.ActivatePostProcessInChild((int)postProcess.BPM_90);
            } 
            else if (cpt >= 95 && cpt < 100)
            {
                cpt = 95;
                PostProcessManager.post.ActivatePostProcessInChild((int)postProcess.BPM_90);
            } 
            else if (cpt >= 100 && cpt < 105)
            {
                cpt = 100;
                PostProcessManager.post.ActivatePostProcessInChild((int)postProcess.BPM_100);
            } 
            else if (cpt >= 105 && cpt < 110)
            {
                cpt = 105;
                PostProcessManager.post.ActivatePostProcessInChild((int)postProcess.BPM_100);
            }
            else if (cpt >= 110 && cpt < 115)
            {
                cpt = 110;
                PostProcessManager.post.ActivatePostProcessInChild((int)postProcess.BPM_100);
            }
            else if (cpt >= 115 && cpt < 120)
            {
                cpt = 115;
                PostProcessManager.post.ActivatePostProcessInChild((int)postProcess.BPM_100);
            }
            else if (cpt >= 120)
            {
                cpt = 120;
                PostProcessManager.post.ActivatePostProcessInChild((int)postProcess.BPM_100);
            }
            
            AudioHelmClock.Instance.bpm = cpt;
            AudioHelmClock.Instance.timeToReach = 60d / cpt * 100d;

            Destroy(this);
        }
        else
        {
            if (started)

                StartCoroutine(WaiForCounting());
            bpm.text = cpt.ToString();

            if (timerFloat >= 0 && canCount)
                timerFloat -= Time.deltaTime;
            //Visuel de la Barre
            timerText.text = Mathf.Floor(timerFloat).ToString();
            gameObject.GetComponent<AudioHelmClock>().bpm = cpt;
            if(morpheus.activeSelf)
            {
                if (Input.GetKeyDown(KeyCode.V))
                {
                    cpt = 80;
                }
                else if(Input.GetKeyDown(KeyCode.B))
                {
                    cpt = 100;
                }
                else if (Input.GetKeyDown(KeyCode.N))
                {
                    cpt = 110;
                }
                if (Input.GetKeyDown(KeyCode.E))
                   StartCoroutine( FadeIn(true));
                else if (Input.GetKeyDown(KeyCode.R))
                    StartCoroutine( FadeIn(false));
            }    
        }

    }


    void DataReceived(string data, UduinoDevice board)
    {
        //xant tu peux ecrire ici
        Debug.Log("Yo");
        if (!started && !doOnce && data.Length != 19)
        {
            cpt = int.Parse(data);
            if (cpt > 150 || cpt < 50)
                cpt = 80;
            doOnce = true;
            started = true;
        }
        if (numbers == null)
            numbers = new List<int>();

        if (canCount)
        {
            cpt = 0;
            numbers.Add(int.Parse(data));

            for (int i = 0; i < numbers.Count; i++)
            {
                if (numbers[i] > 50 && numbers[i] < 130)
                {
                    cpt += numbers[i];
                }
                else
                    cpt += 60;
            }

            if (numbers.Count > 0)
            {

                if (numbers.Count > 1)
                    cpt = cpt / (numbers.Count-1);
                else
                    cpt = cpt / (numbers.Count );
                /*if (numbers[numbers.Count - 1] > cpt + 50 || numbers[numbers.Count - 1] < cpt - 50)
                {
                 
                    numbers.Remove(int.Parse(data));
                    cpt = 0;
                    foreach (int _bpm in numbers)
                    {
                        cpt += _bpm;
                    }
                    if (numbers.Count - 1 != 0)
                        cpt = cpt / (numbers.Count - 1);
                    
                }*/

            }


        }

    }

    private IEnumerator WaiForCounting()
    {
        StartCoroutine(CircleSpawning());
        StartCoroutine(FadeOutSound(sound));
        GetComponent<AudioSource>().Play();
        started = false;
        timerFloat = 20; // to start timer 
        canCount = true;
        yield return new WaitForSeconds(20);
        canCount = false;
        GetComponent<AudioSource>().Stop();

        morpheus.SetActive(true);
       
    }

    private IEnumerator CircleSpawning()
    {
        Instantiate(circleRef, heart.transform.position, Quaternion.identity);
        yield return new WaitForSecondsRealtime(1f);
        
        StartCoroutine(CircleSpawning());
    }

    IEnumerator FadeIn(bool tuto)
    {
        if (!loadOnce)
        {
            loadOnce = true;
            SoundClic.GetComponent<AudioSource>().Play();
            AsyncOperation _scen = new AsyncOperation();
            if (tuto)
            {
               _scen = SceneManager.LoadSceneAsync("Tuto");
                _scen.allowSceneActivation = false;

            }
            else
            {
               _scen=  SceneManager.LoadSceneAsync("Main");
                _scen.allowSceneActivation = false;

            }

            for (float i = 0; i < 250; i++)
            {
                fade.color = new Color(0, 0, 0, i / 255);
                yield return new WaitForSeconds(0.01f);
            }
            _scen.allowSceneActivation = true;
        }

    }

    IEnumerator FadeOutSound(GameObject track)
    {
        if (track.GetComponent<AudioSource>().volume <= 1f)
        {
            track.GetComponent<AudioSource>().volume -= 0.1f;
            yield return new WaitForSeconds(0.2f);
            StartCoroutine(FadeOutSound(track));
        }
    }
}
