using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LeaderBoardDisplay : MonoBehaviour
{
    enum Alphabet { a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z}
    private Alphabet currentLetter;
    public LeaderBoard leaderBoard;
    private TextMeshProUGUI scores;
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

    IEnumerator Start()
    {      

        yield return new WaitForSeconds(4f);
        scores = GetComponent<TextMeshProUGUI>();
        for (int i = 10; i >= 0; i--)
        {
            if (leaderBoard.names[i] != string.Empty)
                scores.text += leaderBoard.names[i] + "      " + leaderBoard.bpm[i].ToString() + "       " + leaderBoard.scores[i].ToString() + "\n";
            else
                scores.text += "aaa      " + leaderBoard.bpm[i].ToString() + "       " + leaderBoard.scores[i].ToString() + "\n";

        }
        yield return new WaitForSeconds(2f);
        Panel.SetActive(true);
        yield return new WaitForSeconds(0.1f);
       
        if (leaderBoard.placeInLeaderBoard < 1000)
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
        if(Input.GetKeyDown(KeyCode.Space) && restart.activeSelf)
        {
            SceneManager.LoadScene("Intro");
        }
        if (leaderBoard.placeInLeaderBoard < 1000)
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
                        leaderBoard.names[leaderBoard.placeInLeaderBoard] = string.Empty;
                        names1[0].gameObject.SetActive(false);
                        names1[2].gameObject.SetActive(false);
                        names2[0].gameObject.SetActive(true);
                        names2[1].gameObject.SetActive(true);
                        names2[2].gameObject.SetActive(true);
                        names2[0].text = "b";
                        names2[1].text = "a";
                        names2[2].text = "z";
                        leaderBoard.names[leaderBoard.placeInLeaderBoard] += names1[1].text;
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
                        leaderBoard.names[leaderBoard.placeInLeaderBoard] += names2[1].text;
                        break;
                    case 2:
                        names3[0].gameObject.SetActive(false);
                        names3[1].gameObject.SetActive(false);
                        names3[2].gameObject.SetActive(false);
                        names1[1].gameObject.SetActive(false);
                        names2[1].gameObject.SetActive(false);
                        leaderBoard.names[leaderBoard.placeInLeaderBoard] += names3[1].text;
                        scores.text = string.Empty;

                        for (int i = 10 - 1; i >= 0; i--)
                        {
                            if(leaderBoard.names[i] != string.Empty)
                            scores.text += leaderBoard.names[i] + "      " + leaderBoard.bpm[i].ToString() + "       " + leaderBoard.scores[i].ToString() + "\n";
                            else
                                scores.text += "aaa      " + leaderBoard.bpm[i].ToString() + "       " + leaderBoard.scores[i].ToString() + "\n";

                        }

                        if(leaderBoard.placeInLeaderBoard > 10)
                        {
                            top10.SetActive(true);
                            top10Rank.text = leaderBoard.placeInLeaderBoard.ToString();
                            top10Score.text = leaderBoard.scores[ leaderBoard.placeInLeaderBoard].ToString();
                            top10Bpm.text = leaderBoard.bpm[ leaderBoard.placeInLeaderBoard].ToString();
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
