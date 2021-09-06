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

    [Header("Special")]
    private bool specialOpen;
    private int cptSpecial;
    private int numberSpecial;
    private bool loadOnce;
    public GameObject[] names1 = new GameObject[3];
    public GameObject[] names2 = new GameObject[3];
    public GameObject[] names3 = new GameObject[3];
    public GameObject Panel;

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
            if(cpt>=60 && cpt<65)
            {
                AudioHelmClock.Instance.bpm = 60;
            }
            else if (cpt >= 65 && cpt < 70)
            {
                AudioHelmClock.Instance.bpm = 65;
            }
            else if (cpt >= 70 && cpt < 75)
            {
                AudioHelmClock.Instance.bpm = 70;
            }
            else if (cpt >= 75 && cpt < 80)
            {
                AudioHelmClock.Instance.bpm = 75;
            }
            else if (cpt >= 80 && cpt < 85)
            {
                AudioHelmClock.Instance.bpm = 80;
            }
            else if (cpt >= 85 && cpt < 90)
            {
                AudioHelmClock.Instance.bpm = 85;
            }
            else if (cpt >= 90 && cpt < 95)
            {
                AudioHelmClock.Instance.bpm = 90;
            }
            else if (cpt >= 95 && cpt < 100)
            {
                AudioHelmClock.Instance.bpm = 95;
            }
            else if (cpt >= 100 && cpt < 105)
            {
                AudioHelmClock.Instance.bpm = 100;
            }
            else if (cpt >= 105 && cpt < 110)
            {
                AudioHelmClock.Instance.bpm = 105;
            }
            else if (cpt >= 110 && cpt < 115)
            {
                AudioHelmClock.Instance.bpm = 110;
            }
            else if (cpt >= 115 && cpt < 120)
            {
                AudioHelmClock.Instance.bpm = 115;
            }
            else if (cpt >= 120 && cpt < 130)
            {
                AudioHelmClock.Instance.bpm = 120;
            }


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
            if (!specialOpen)
            {

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
                    else if (Input.GetKeyDown(KeyCode.Space))
                    {
                        Panel.SetActive(true);
                        names1[0].gameObject.SetActive(true);
                        names1[1].gameObject.SetActive(true);
                        cptSpecial = 0;
                        ChangeLetter(names1,1);
                        specialOpen = true;
                    }
                }
                else
                {
                    if (Input.GetKeyDown(KeyCode.E))
                       StartCoroutine( FadeIn(true));
                    else if (Input.GetKeyDown(KeyCode.R))
                        StartCoroutine( FadeIn(false));
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    switch (numberSpecial)
                    {
                        case 0:
                            if (cptSpecial != 1)
                                cptSpecial++;
                            else
                                cptSpecial = 0;
                            ChangeLetter(names1,1);
                            break;
                        case 1:
                            if(int.Parse( names1[1].GetComponentInChildren<TextMeshProUGUI>().text) == 1)
                            {
                                if (cptSpecial != 2)
                                    cptSpecial++;
                                else
                                    cptSpecial = 0;
                            }
                            else
                            {
                                if (cptSpecial != 9)
                                    cptSpecial++;
                                else
                                    cptSpecial = 6;
                            }
                            ChangeLetter(names2,2);
                            break;
                        case 2:
                            if (cptSpecial != 9)
                                cptSpecial++;
                            else
                                cptSpecial = 0;
                            ChangeLetter(names3, 3);
                            break;


                    }
                }
                else if (Input.GetKeyDown(KeyCode.R))
                {
                   
                    switch (numberSpecial)
                    {
                        case 0:
                            if (cptSpecial != 0)
                                cptSpecial--;
                            else
                                cptSpecial = 1;
                            ChangeLetter(names1, 1);
                            break;
                        case 1:
                            if (int.Parse(names1[1].GetComponentInChildren<TextMeshProUGUI>().text) == 1)
                            {
                                if (cptSpecial != 0)
                                    cptSpecial--;
                                else
                                    cptSpecial = 2;
                            }
                            else
                            {
                                if (cptSpecial != 6)
                                    cptSpecial--;
                                else
                                    cptSpecial = 9;
                            }
                            ChangeLetter(names2, 2);
                            break;
                        case 2:
                            if (cptSpecial != 0)
                                cptSpecial--;
                            else
                                cptSpecial = 9;
                            ChangeLetter(names3, 3);
                            break;

                    }
                }
                else if (Input.GetKeyDown(KeyCode.Space))
                {
                    switch (numberSpecial)
                    {
                        case 0:
                            names1[0].gameObject.SetActive(false);
                            names1[2].gameObject.SetActive(false);
                            names2[0].gameObject.SetActive(true);
                            names2[1].gameObject.SetActive(true);
                            names2[2].gameObject.SetActive(true);
                            if(int.Parse(names1[1].GetComponentInChildren<TextMeshProUGUI>().text) == 0)
                            {
                                names2[0].GetComponentInChildren<TextMeshProUGUI>().text = "7";
                                names2[1].GetComponentInChildren<TextMeshProUGUI>().text = "6";
                                names2[2].GetComponentInChildren<TextMeshProUGUI>().text = "9";
                                cptSpecial = 6;
                            }
                            else
                            {
                                names2[0].GetComponentInChildren<TextMeshProUGUI>().text = "1";
                                names2[1].GetComponentInChildren<TextMeshProUGUI>().text = "0";
                                names2[2].GetComponentInChildren<TextMeshProUGUI>().text = "2";
                                cptSpecial = 0;
                            }
                            break;
                        case 1:
                            names2[0].gameObject.SetActive(false);
                            names2[2].gameObject.SetActive(false);
                            names3[0].gameObject.SetActive(true);
                            names3[1].gameObject.SetActive(true);
                            names3[2].gameObject.SetActive(true);
                            names3[0].GetComponentInChildren<TextMeshProUGUI>().text = "1";
                            names3[1].GetComponentInChildren<TextMeshProUGUI>().text = "0";
                            names3[2].GetComponentInChildren<TextMeshProUGUI>().text = "9";
                            cptSpecial = 0;
                            break;
                        case 2:
                            names2[0].gameObject.SetActive(false);
                            names2[2].gameObject.SetActive(false);
                            cpt = int.Parse(names1[1].GetComponentInChildren<TextMeshProUGUI>().text) * 100 + int.Parse(names2[1].GetComponentInChildren<TextMeshProUGUI>().text) * 10 + int.Parse(names3[1].GetComponentInChildren<TextMeshProUGUI>().text);
                           
                            Panel.SetActive(false);
                            lunch = true;
                            specialOpen = false;
                            cptSpecial = 0;
                            break;
                        default:
                            break;
                    }
                    numberSpecial++;
               

                }
                else if(Input.GetKeyDown(KeyCode.Q))
                {
                    names1[0].gameObject.SetActive(false);
                    names1[1].gameObject.SetActive(false);
                    names1[2].gameObject.SetActive(false);
                    names2[0].gameObject.SetActive(false);
                    names2[1].gameObject.SetActive(false);
                    names2[2].gameObject.SetActive(false);
                    specialOpen = false;
                    Panel.SetActive(false);
                }
            }
            //}    
        }

    }
    private void ChangeLetter(GameObject[] currentList, int number)
    {

        switch (number)
        {
            case 1:               
                    currentList[0].GetComponentInChildren<TextMeshProUGUI>().text = (Mathf.Abs( cptSpecial-1)).ToString();
                    currentList[1].GetComponentInChildren<TextMeshProUGUI>().text = (cptSpecial).ToString();
                break;
            case 2:
                if(int.Parse( names1[1].GetComponentInChildren<TextMeshProUGUI>().text) == 1)
                {
                    if (cptSpecial == 2)
                    {
                        currentList[0].GetComponentInChildren<TextMeshProUGUI>().text = (cptSpecial - 2).ToString();
                        currentList[1].GetComponentInChildren<TextMeshProUGUI>().text = (cptSpecial).ToString();
                        currentList[2].GetComponentInChildren<TextMeshProUGUI>().text = (cptSpecial - 1).ToString();

                    }
                    else if (cptSpecial == 0)
                    {
                        currentList[0].GetComponentInChildren<TextMeshProUGUI>().text = (cptSpecial + 1).ToString();
                        currentList[1].GetComponentInChildren<TextMeshProUGUI>().text = (cptSpecial).ToString();
                        currentList[2].GetComponentInChildren<TextMeshProUGUI>().text = (2).ToString();
                    }
                    else
                    {
                        currentList[0].GetComponentInChildren<TextMeshProUGUI>().text = (cptSpecial + 1).ToString();
                        currentList[1].GetComponentInChildren<TextMeshProUGUI>().text = (cptSpecial).ToString();
                        currentList[2].GetComponentInChildren<TextMeshProUGUI>().text = (cptSpecial - 1).ToString();

                    }
                }
                else
                {
                    if (cptSpecial == 9)
                    {
                        currentList[0].GetComponentInChildren<TextMeshProUGUI>().text = (cptSpecial - 3).ToString();
                        currentList[1].GetComponentInChildren<TextMeshProUGUI>().text = (cptSpecial).ToString();
                        currentList[2].GetComponentInChildren<TextMeshProUGUI>().text = (cptSpecial - 1).ToString();

                    }
                    else if (cptSpecial == 6)
                    {
                        currentList[0].GetComponentInChildren<TextMeshProUGUI>().text = (cptSpecial + 1).ToString();
                        currentList[1].GetComponentInChildren<TextMeshProUGUI>().text = (cptSpecial).ToString();
                        currentList[2].GetComponentInChildren<TextMeshProUGUI>().text = (9).ToString();
                    }
                    else
                    {
                        currentList[0].GetComponentInChildren<TextMeshProUGUI>().text = (cptSpecial + 1).ToString();
                        currentList[1].GetComponentInChildren<TextMeshProUGUI>().text = (cptSpecial).ToString();
                        currentList[2].GetComponentInChildren<TextMeshProUGUI>().text = (cptSpecial - 1).ToString();

                    }
                }
                break;
            default:
                if (cptSpecial == 9)
                {
                    currentList[0].GetComponentInChildren<TextMeshProUGUI>().text = (cptSpecial - 9).ToString();
                    currentList[1].GetComponentInChildren<TextMeshProUGUI>().text = (cptSpecial).ToString();
                    currentList[2].GetComponentInChildren<TextMeshProUGUI>().text = (cptSpecial - 1).ToString();

                }
                else if (cptSpecial == 0)
                {
                    currentList[0].GetComponentInChildren<TextMeshProUGUI>().text = (cptSpecial + 1).ToString();
                    currentList[1].GetComponentInChildren<TextMeshProUGUI>().text = (cptSpecial).ToString();
                    currentList[2].GetComponentInChildren<TextMeshProUGUI>().text = (9).ToString();
                }
                else
                {
                    currentList[0].GetComponentInChildren<TextMeshProUGUI>().text = (cptSpecial + 1).ToString();
                    currentList[1].GetComponentInChildren<TextMeshProUGUI>().text = (cptSpecial).ToString();
                    currentList[2].GetComponentInChildren<TextMeshProUGUI>().text = (cptSpecial - 1).ToString();

                }
                break;
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
            yield return new WaitUntil(() => _scen.progress >= 0.9f);
            _scen.allowSceneActivation = true;
        }

    }

    IEnumerator FadeOutSound(GameObject track)
    {
        if (track &&track.GetComponent<AudioSource>().volume <= 1f)
        {
            yield return new WaitForSeconds(0.2f);
            StartCoroutine(FadeOutSound(track));
        }
    }
}
