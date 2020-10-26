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
    private bool doOnce;
    private bool canCount;
    private int cpt;





    private void Start()
    {
        StartCoroutine("CircleSpawning");

        timerFloat = 20;// to enable timer only when started = true
        UduinoManager.Instance.OnDataReceived += DataReceived;
        DontDestroyOnLoad(this);

    }

    private void Update()
    {
      
        if (SceneManager.GetActiveScene().name == "MainScene")
        {
            AudioHelmClock.GetInstance().bpm = cpt;
            AudioHelmClock.GetInstance().SetGlobalBpm();
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
            gameObject.GetComponent<AudioHelmClock>().SetGlobalBpm();
        }
    }


    void DataReceived(string data, UduinoDevice board)
    {
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

            foreach (int _bpm in numbers)
            {
                cpt += _bpm;
            }

            if (numbers.Count > 0)
            {

                if (numbers.Count > 1)
                    cpt = cpt / (numbers.Count-1);
                else
                    cpt = cpt / (numbers.Count );
                if (numbers[numbers.Count - 1] > cpt + 50 || numbers[numbers.Count - 1] < cpt - 50)
                {
                   
                    numbers.Remove(int.Parse(data));
                    cpt = 0;
                    foreach (int _bpm in numbers)
                    {
                        cpt += _bpm;
                    }
                    if (numbers.Count - 1 != 0)
                        cpt = cpt / (numbers.Count - 1);

                }

            }


        }

    }

    private IEnumerator WaiForCounting()
    {
        GetComponent<AudioSource>().Play();
        started = false;
        timerFloat = 20; // to start timer 
        canCount = true;
        yield return new WaitForSeconds(20);
        canCount = false;
        GetComponent<AudioSource>().Stop();
        SceneManager.LoadScene("MainScene");
    }

    private IEnumerator CircleSpawning()
    {
        Instantiate(circleRef, heart.transform.position, Quaternion.identity);
        yield return new WaitForSecondsRealtime(1f);
        StartCoroutine("CircleSpawning");
    }

    

}
