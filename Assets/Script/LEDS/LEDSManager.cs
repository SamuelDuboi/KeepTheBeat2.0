using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Uduino;
public class LEDSManager : Singleton<LEDSManager>
{
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 6; i++)
        {
            if (i < 1)
            {
                UduinoManager.Instance.sendCommand("turnOnPixel", i, 255, 255, 0, 100);
            }
            else if (i>=1 && i < 2)
            {
                UduinoManager.Instance.sendCommand("turnOnPixel", i, 135, 255, 0, 100);
            }
            else if (i >= 2 && i < 3)
            {
                UduinoManager.Instance.sendCommand("turnOnPixel", i, 255, 0, 0, 100);
            }
            else if (i >= 3 && i < 4)
            {
                UduinoManager.Instance.sendCommand("turnOnPixel", i, 0, 255, 255, 100);
            }
            else if (i >= 4 && i < 5)
            {
                UduinoManager.Instance.sendCommand("turnOnPixel", i, 136, 0, 255, 100);
            }
            else
            {
                UduinoManager.Instance.sendCommand("turnOnPixel", i, 248, 90, 205, 100);
            }
        }
        
    }


    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
            LightUp(0);
    }
    public void LightUp(int row)
    {
        StartCoroutine(LightDown(row));
    }

  private  IEnumerator LightDown(int row)
    {
        for (int i = row*6 ; i < 6 +row*6; i++)
        {
            UduinoManager.Instance.sendCommand("changeBrightnessPixel", i,200);
        }
        yield return new WaitForSeconds(0.5f);// this time will be change
        for (int i = row * 6; i < 6 + row * 6; i++)
        {
            UduinoManager.Instance.sendCommand("changeBrightnessPixel", i, 100);
        }
    }
}
