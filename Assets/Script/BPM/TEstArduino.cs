using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Uduino;
using System;

public class TEstArduino : MonoBehaviour
{

    // Pulse Sensor purple wire connected to analog pin A0
    int blinkPin = 13;                // pin to blink led at each beat

    // Volatile Variables, used in the interrupt service routine!
    volatile int BPM;                   // int that holds raw Analog in 0. updated every 2mS
    volatile int Signal;                // holds the incoming raw data
    volatile int IBI = 600;             // int that holds the time interval between beats! Must be seeded! 
    volatile bool Pulse = false;     // "True" when User's live heartbeat is detected. "False" when not a "live beat". 
    volatile bool QS = false;        // becomes true when Arduoino finds a beat.

    static bool serialVisual = true;   // Set to 'false' by Default.  Re-set to 'true' to see Arduino Serial Monitor ASCII Visual Pulse 

    private float timer;

    volatile int[] rate = new int[10];                      // array to hold last ten IBI values
    volatile int sampleCounter = 0;          // used to determine pulse timing
    volatile int lastBeatTime = 0;           // used to find IBI
    volatile int P = 512;                      // used to find peak in pulse wave, seeded
    volatile int T = 512;                     // used to find trough in pulse wave, seeded
    volatile int thresh = 525;                // used to find instant moment of heart beat, seeded
    volatile int amp = 100;                   // used to hold amplitude of pulse waveform, seeded
    volatile bool firstBeat = true;        // used to seed rate array so we startup with reasonable BPM
    volatile bool secondBeat = false;      // used to seed rate array so we startup with reasonable BPM
    // Start is called before the first frame update
    void Start()
    {
        UduinoManager.Instance.pinMode(blinkPin, PinMode.Output);
        UduinoManager.Instance.pinMode(AnalogPin.A0, PinMode.Input);

        
    }
   
    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 0.002)
        {
            timer = 0;
            TestBeat();
        }
       

       
    }

  
   void TestBeat() //triggered when Timer2 counts to 124
    {
        Signal = UduinoManager.Instance.analogRead(AnalogPin.A0);// read the Pulse Sensor 
        sampleCounter += 2;                         // c'est genre un time.deltatime +2 car tout les 2 miliseconde
        int N = sampleCounter - lastBeatTime;       // monitor the time since the last beat to avoid noise

        #region regarde les reponse les plus basse et les plus haute
        //  find the peak and trough of the pulse wave
        if (Signal < thresh && N > (IBI / 5) * 3) // avoid dichrotic noise by waiting 3/5 of last IBI au début c'est egal a 3600 soit une minute puis c'est egale au temps entre les bits
        {
            if (Signal < T) // T is the trough
            {
                T = Signal; // keep track of lowest point in pulse wave  en théori ca devrait pas des masses bouger, c'est pour qualibré la réponse
            }
        }

        if (Signal > thresh && Signal > P)
        {          // thresh condition helps avoid noise
            P = Signal;                             // P is the peak
        }                                        // keep track of highest point in pulse wave
        #endregion

        //  NOW IT'S TIME TO LOOK FOR THE HEART BEAT
        // signal surges up in value every time there is a pulse
        if (N > 250)
        {                                   // avoid high frequency noise
            if ((Signal > thresh) && (Pulse == false) && (N > (IBI / 5) * 3))
            {
                Pulse = true;                               // set the Pulse flag when we think there is a pulse

                //en soit, ca je m'en fous
                UduinoManager.Instance.digitalWrite(blinkPin, State.HIGH);                // turn on pin 13 LED


                IBI = sampleCounter - lastBeatTime;         // measure time between beats in mS
                lastBeatTime = sampleCounter;               // keep track of time for next pulse

                if (secondBeat)
                {                        // if this is the second beat, if secondBeat == TRUE
                    secondBeat = false;                  // clear secondBeat flag
                    for (int i = 0; i <= 9; i++) // seed the running total to get a realisitic BPM at startup
                    {
                        rate[i] = IBI;
                    }
                }

                if (firstBeat) // if it's the first time we found a beat, if firstBeat == TRUE
                {
                    firstBeat = false;                   // clear firstBeat flag
                    secondBeat = true;                   // set the second beat flag
                    return;                              // IBI value is unreliable so discard it
                }
                // keep a running total of the last 10 IBI values
                var  runningTotal = 0;                  // clear the runningTotal variable    

                for (int i = 0; i <= 8; i++)
                {                // shift data in the rate array
                    rate[i] = rate[i + 1];                  // and drop the oldest IBI value 
                    runningTotal += rate[i];              // add up the 9 oldest IBI values
                }

                rate[9] = IBI;                          // add the latest IBI to the rate array
                runningTotal += rate[9];                // add the latest IBI to runningTotal
                runningTotal /= 10;                     // average the last 10 IBI values 
                BPM = 60000 / runningTotal;
                Debug.Log(BPM);// how many beats can fit into a minute? that's BPM!
                QS = true;                              // set Quantified Self flag 
                                                        // QS FLAG IS NOT CLEARED INSIDE THIS ISR
            }
        }
    } 
}
