using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Uduino;
public class LEDSManager : Singleton<LEDSManager>
{
    protected override void Awake()
    {
        base.Awake();
    }
    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return new WaitForSeconds(2f);
        for (int i = 0; i < 41; i++)
        {
            if (i < 7)
            {
                UduinoManager.Instance.sendCommand("turnOn", i, 248, 90, 205, 150);

            }
            else if (i>=7 && i < 13)
            {
                 UduinoManager.Instance.sendCommand("turnOn", i, 136, 0, 255, 150);
            }
            else if (i >= 13 && i < 19)
            {
                 UduinoManager.Instance.sendCommand("turnOn", i, 0, 255, 255, 150);
            }
            else if (i >= 19 && i < 23)
            {
                UduinoManager.Instance.sendCommand("turnOn", i, 255, 255, 255, 150);
            }
            else if (i >= 23 && i < 29)
            {
                UduinoManager.Instance.sendCommand("turnOn", i, 255, 0, 0, 150);
            }
            else if (i >= 29 && i < 35)
            {
                UduinoManager.Instance.sendCommand("turnOn", i, 135, 255, 0, 150);
            }
            else
            {
                UduinoManager.Instance.sendCommand("turnOn", i, 255, 255, 0, 150);
            }
            
            yield return new WaitForSeconds(0.05f);
        }
        
    }

    public void LightUp(int row)
    {
        StartCoroutine(LightDown(row));
    }

  private  IEnumerator LightDown(int row)
    {
        switch (row)
        {
            case 0:
                for (int i = 0; i < 7; i++)
                {

                    UduinoManager.Instance.sendCommand("turnOn", i, 248, 90, 205, 10);

                    yield return new WaitForSeconds(0.05f);
                }
                yield return new WaitForSeconds(0.1f);
                for (int i = 0; i < 7; i++)
                {
                    UduinoManager.Instance.sendCommand("turnOn", i, 248, 90, 205, 150);
                    yield return new WaitForSeconds(0.05f);
                }
                break;
            case 1:
                for (int i = 7; i < 13; i++)
                {
                    UduinoManager.Instance.sendCommand("turnOn", i, 136, 0, 255, 10);
                    yield return new WaitForSeconds(0.05f);
                }
                yield return new WaitForSeconds(0.1f);
                for (int i = 7; i < 13; i++)
                {
                     UduinoManager.Instance.sendCommand("turnOn", i, 136, 0, 255, 150);
                    yield return new WaitForSeconds(0.05f);
                }
                break;
            case 2:
                for (int i = 13; i < 19; i++)
                {
                    
                    UduinoManager.Instance.sendCommand("turnOn", i, 0, 255, 255, 10);
                    yield return new WaitForSeconds(0.05f);
                }
                yield return new WaitForSeconds(0.1f);
                for (int i = 13; i < 19; i++)
                {
                    UduinoManager.Instance.sendCommand("turnOn", i, 0, 255, 255, 150);
                    yield return new WaitForSeconds(0.05f);
                }
                break;

            case 3:
                for (int i = 23; i < 29; i++)
                {
                    UduinoManager.Instance.sendCommand("turnOn", i, 255, 0, 0, 10);

                    yield return new WaitForSeconds(0.05f);
                }
                yield return new WaitForSeconds(0.1f);
                for (int i = 23; i < 29; i++)
                {
                    UduinoManager.Instance.sendCommand("turnOn", i, 255, 0, 0, 150);

                    yield return new WaitForSeconds(0.05f);
                }
                break;
            case 4:
                for (int i = 29; i < 35; i++)
                {
                    UduinoManager.Instance.sendCommand("turnOn", i, 135, 255, 0, 10);
                    yield return new WaitForSeconds(0.05f);
                }
                yield return new WaitForSeconds(0.1f);
                for (int i = 29; i < 35; i++)
                {
                    UduinoManager.Instance.sendCommand("turnOn", i, 135, 255, 0, 150);
                    yield return new WaitForSeconds(0.05f);
                }
                break;
            case 5:
                for (int i = 35; i < 41; i++)
                {
                    UduinoManager.Instance.sendCommand("turnOn", i, 255, 255, 0, 10);
                    yield return new WaitForSeconds(0.05f);
                }
                yield return new WaitForSeconds(0.1f);
                for (int i = 35; i < 41; i++)
                {
                    UduinoManager.Instance.sendCommand("turnOn", i, 255, 255, 0, 150);
                    yield return new WaitForSeconds(0.05f);
                }
                break;
            case 6:
                for (int i = 19; i < 23; i++)
                {
                    UduinoManager.Instance.sendCommand("turnOn", i, 255, 255, 255, 10);
                    yield return new WaitForSeconds(0.05f);
                }
                yield return new WaitForSeconds(0.1f);
                for (int i = 19; i < 23; i++)
                {
                    UduinoManager.Instance.sendCommand("turnOn", i, 255, 255, 255, 150);
                    yield return new WaitForSeconds(0.05f);
                }
                break;
            default:
                break;
        }
    }
}
