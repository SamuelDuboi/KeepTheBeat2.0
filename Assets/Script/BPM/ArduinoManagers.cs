﻿using AudioHelm;
using System.Collections;
using System.Collections.Generic;
using Uduino;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ArduinoManagers : Singleton<ArduinoManagers>
{
    public Text text;
    public Text title;
    public Image time;
    public string text1;
    public string text2;
    public float timer;
    private List<int> numbers;
    private bool started;
    private bool doOnce;
    private bool canCount;
    private int cpt;



    private void Start()
    {

        timer = -1;// to enable timer only when started = true
        UduinoManager.Instance.OnDataReceived += DataReceived;
        DontDestroyOnLoad(this);

    }

    private void Update()
    {
      
        if (SceneManager.GetActiveScene().name == "TestHelm")
        {
            AudioHelmClock.GetInstance().bpm = cpt;
            AudioHelmClock.GetInstance().SetGlobalBpm();
            Destroy(this);
        }
        else
        {
            if (started)
                StartCoroutine(WaiForCounting());
            text.text = cpt.ToString();

            if (timer >= 0)
                timer += Time.deltaTime;
            time.fillAmount = timer / 20;
            title.text = text2 + ((int)(20 - timer)).ToString() + " secondes";
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
        timer = 0; // to start timer 
        canCount = true;
        yield return new WaitForSeconds(20);
        canCount = false;
        GetComponent<AudioSource>().Stop();
        SceneManager.LoadScene("TestHelm");
    }

}