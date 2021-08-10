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
    public TextMeshProUGUI bpm;
    public TextMeshProUGUI instructions;
    public GameObject circleRef;
    public GameObject heart;

    [Header("Values")]
    private List<int> numbers;
    private bool started;
    [SerializeField] private bool doOnce;
    private bool canCount;
    private double cpt;
    private bool lunch;

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
       // UduinoManager.Instance.OnDataReceived += DataReceived;
        DontDestroyOnLoad(this);
        started = true;

    }

    private void Update()
    {
      
        if (SceneManager.GetActiveScene().name == "Main" || SceneManager.GetActiveScene().name == "Tuto")
        {           
            AudioHelmClock.Instance.bpm = cpt;
            Destroy(this);
        }
        else
        {
            if (started)
                StartCoroutine(WaiForCounting());
            bpm.text = cpt.ToString();


            //Visuel de la Barre
            gameObject.GetComponent<AudioHelmClock>().bpm = cpt;
            //if(morpheus.activeSelf)
            //{
            if (!lunch)
            {
               
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    cpt = 65;
                    lunch = true;
                }
                else if (Input.GetKeyDown(KeyCode.Z))
                {
                    cpt = 70;
                    lunch = true;
                }
                else if (Input.GetKeyDown(KeyCode.E))
                {
                    cpt = 80;
                    lunch = true;
                }
                else if (Input.GetKeyDown(KeyCode.R))
                {
                    cpt = 90;
                    lunch = true;
                }
                else if (Input.GetKeyDown(KeyCode.T))
                {
                    cpt = 100;
                    lunch = true;
                }
                else if (Input.GetKeyDown(KeyCode.H))
                {
                    cpt = 110;
                    lunch = true;
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.E))
                   StartCoroutine( FadeIn(true));
                else if (Input.GetKeyDown(KeyCode.R))
                    StartCoroutine( FadeIn(false));
            }
            //}    
        }

    }


  /*  void DataReceived(string data, UduinoDevice board)
    {
        //xant tu peux ecrire ici
       // Debug.Log("Yo");
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

            }


        }

    }*/

    private IEnumerator WaiForCounting()
    {
        StartCoroutine(CircleSpawning());
        StartCoroutine(FadeOutSound(sound));
        GetComponent<AudioSource>().Play();
        started = false;
        canCount = true;
        yield return new WaitUntil(()=>lunch);
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
        if (track &&track.GetComponent<AudioSource>().volume <= 1f)
        {
            track.GetComponent<AudioSource>().volume -= 0.1f;
            yield return new WaitForSeconds(0.2f);
            StartCoroutine(FadeOutSound(track));
        }
    }
}
