using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Uduino;
public class LeaderBoardDisplay : MonoBehaviour
{
    enum Alphabet { a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z}
    private Alphabet currentLetter;
    public TextMeshProUGUI names;
    public TextMeshProUGUI bpms;
    public TextMeshProUGUI scores;
    private int cpt;
    private int letterNumber;
    public TextMeshProUGUI[]names1 = new TextMeshProUGUI[3];
    public TextMeshProUGUI[]names2 = new TextMeshProUGUI[3];
    public TextMeshProUGUI[]names3 = new TextMeshProUGUI[3];
    public GameObject restart;
    public GameObject Panel;
    public GameObject cadre;
    public GameObject title;
    public GameObject value;
    public GameObject top10;
    public TextMeshProUGUI top10Rank;
    public TextMeshProUGUI top10Name;
    public TextMeshProUGUI top10Score;
    public TextMeshProUGUI top10Bpm;
    private bool doOnce;

  private  IEnumerator Start()
    {
        
        yield return new WaitForSeconds(4f);
        for (int i = 0; i <10; i++)
        {
            if (LeaderBoard.instance.names[i] != string.Empty)
            {
                names.text += LeaderBoard.instance.names[i] + "\n";
                bpms.text += LeaderBoard.instance.bpm[i].ToString() + "\n";
                scores.text += LeaderBoard.instance.scores[i].ToString() + "\n";
            }
            else
            {
                names.text += "aaa" + "\n";
                bpms.text += LeaderBoard.instance.bpm[i].ToString() + "\n";
                scores.text += LeaderBoard.instance.scores[i].ToString() + "\n";
            }

        }
        yield return new WaitForSeconds(2f);
        Panel.SetActive(true);
        yield return new WaitForSeconds(0.1f);
       
        if (LeaderBoard.instance.placeInLeaderBoard < 1000)
        {            
            for (int i = 0; i < 3 ; i++)
            {
                names1[i].gameObject.SetActive(true);                
            }
            names1[0].text = "b";
            names1[1].text = "a";
            names1[2].text = "z";
        }
        else
        {
            Panel.SetActive(false);
            restart.SetActive(true);
        }


    }

    private void Update()
    {
        if(Input.anyKeyDown && restart.activeSelf)
        {
            for (int i = 0; i < 41; i++)
            {
              UduinoManager.Instance.sendCommand("turnOn", i, 0, 0, 0, 150);
            }
            Score.Instance.ScoreDown(100000000);
            SceneManager.LoadScene("Intro");
        }
        if (LeaderBoard.instance.placeInLeaderBoard < 1000)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (cpt != 25)
                    cpt++;
                else
                    cpt = 0;
                switch (letterNumber)
                {
                    case 0:
                        ChangeLetter(names1);
                        break;
                    case 1:
                        ChangeLetter(names2);
                        break;
                    case 2:
                        ChangeLetter(names3);
                        break;

                }
            }
            else if (Input.GetKeyDown(KeyCode.R))
            {
                if (cpt != 0)
                    cpt--;
                else
                    cpt = 25;
                switch (letterNumber)
                {
                    case 0:
                        ChangeLetter(names1);
                        break;
                    case 1:
                        ChangeLetter(names2);
                        break;
                    case 2:
                        ChangeLetter(names3);
                        break;

                }                
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                switch (letterNumber)
                {
                    case 0:
                        LeaderBoard.instance.names[LeaderBoard.instance.placeInLeaderBoard] = string.Empty;
                        names1[0].gameObject.SetActive(false);
                        names1[2].gameObject.SetActive(false);
                        names2[0].gameObject.SetActive(true);
                        names2[1].gameObject.SetActive(true);
                        names2[2].gameObject.SetActive(true);
                        names2[0].text = "b";
                        names2[1].text = "a";
                        names2[2].text = "z";
                        break;
                    case 1:
                        names2[0].gameObject.SetActive(false);
                        names2[2].gameObject.SetActive(false);
                        names3[0].gameObject.SetActive(true);
                        names3[1].gameObject.SetActive(true);
                        names3[2].gameObject.SetActive(true);
                        names3[0].text = "b";
                        names3[1].text = "a";
                        names3[2].text = "z";
                        break;
                    case 2:
                        if (!doOnce)
                        {
                            doOnce = true;
                            LeaderBoard.instance.names[LeaderBoard.instance.placeInLeaderBoard]= names1[1].text + names2[1].text + names3[1].text;
                            LeaderBoard.instance.SaveByXML();
                        }
                        names3[0].gameObject.SetActive(false);
                        names3[1].gameObject.SetActive(false);
                        names3[2].gameObject.SetActive(false);
                        names1[1].gameObject.SetActive(false);
                        names2[1].gameObject.SetActive(false);

                        scores.text = string.Empty;
                        names.text = string.Empty;
                        bpms.text = string.Empty;
                        for (int i = 0; i<10; i++)
                        {
                            if (LeaderBoard.instance.names[i] != string.Empty)
                            {
                                names.text += LeaderBoard.instance.names[i] + "\n";
                                bpms.text += LeaderBoard.instance.bpm[i].ToString() + "\n";
                                scores.text += LeaderBoard.instance.scores[i].ToString() + "\n";
                            }
                            else
                            {
                                names.text += "aaa" + "\n";
                                bpms.text += LeaderBoard.instance.bpm[i].ToString() + "\n";
                                scores.text += LeaderBoard.instance.scores[i].ToString() + "\n";
                            }

                        }


                        Panel.SetActive(false);
                        restart.SetActive(true);
                        break;
                    default:
                        break;
                }
                letterNumber++;
                cpt = 0;
               
            }
        }
    }

    private void ChangeLetter(TextMeshProUGUI[] currentList)
    {
        if(cpt == 25)
        {
            currentList[0].text = (currentLetter +cpt-25).ToString();
            currentList[1].text = (currentLetter + cpt).ToString();
            currentList[2].text = (currentLetter + cpt - 1).ToString();

        }
        else if(cpt == 0)
        {
            currentList[0].text = (currentLetter +cpt +1).ToString();
            currentList[1].text = (currentLetter + cpt).ToString();
            currentList[2].text = (currentLetter + 25).ToString();
        }
      else
        {
            currentList[0].text = (currentLetter + cpt + 1).ToString();
            currentList[1].text = (currentLetter + cpt ).ToString();
            currentList[2].text = (currentLetter + cpt -1).ToString();

        }
    }



}
