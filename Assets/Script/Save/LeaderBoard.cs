using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using UnityEngine;

public class LeaderBoard : MonoBehaviour
{
    public List<float> scores = new List<float>();
    public List<string> names = new List<string>();
    public List<int> bpm =  new List<int>();
    public int placeInLeaderBoard;
    public static LeaderBoard instance;

    public bool victory;
    public bool gameOver;
    public string path;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        LoadByXML();
    }
    public void SaveByXML()
    {
        Save save = CreatSave();
        XmlDocument xmlDocument = new XmlDocument();


        XmlElement rooot = xmlDocument.CreateElement("Save");
        rooot.SetAttribute("FileName", "File_01");
         //save score array
        for (int i = 0; i < 10; i++)
        {
            XmlElement scoreArray = xmlDocument.CreateElement("scoreArray"+i);
            scoreArray.InnerText = save.scoreArray[i].ToString();
            rooot.AppendChild(scoreArray);
        }
        //save bpm array
        for (int i = 0; i < 10; i++)
        {
            XmlElement bpmArray = xmlDocument.CreateElement("bpmArray"+i);
            bpmArray.InnerText = save.bpmArray[i].ToString();
            rooot.AppendChild(bpmArray);
        }

        //saveNames
        for (int i = 0; i < 10; i++)
        {
            XmlElement nameArray = xmlDocument.CreateElement("nameArray"+i);
            nameArray.InnerText = save.nameArray[i].ToString();
            rooot.AppendChild(nameArray);
        }

        xmlDocument.AppendChild(rooot);
        xmlDocument.Save(Application.persistentDataPath + "/DataXML.text");
        if (File.Exists(Application.persistentDataPath + "/DataXML.text"))
        {
            Debug.Log("file save");
        }

    }

    private void LoadByXML()
    {
        if (File.Exists(Application.persistentDataPath + "/DataXML.text"))
        {
            path = Application.persistentDataPath;
            Save save = new Save();

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(Application.persistentDataPath + "/DataXML.text");

            // set scores
            for (int i = 0; i < 10; i++)
            {
                XmlNodeList scoreArray = xmlDocument.GetElementsByTagName("scoreArray"+i);
                scores[i] = int.Parse(scoreArray[0].InnerText);
            }
                save.scoreArray = scores.ToArray();

            for (int i = 0; i < 10; i++)
            {
                //set bpm array
                XmlNodeList bpmArray = xmlDocument.GetElementsByTagName("bpmArray" + i);
                bpm[i] = int.Parse(bpmArray[0].InnerText);
            }
            save.bpmArray = bpm.ToArray();

            //set name array
            for (int i = 0; i < 10; i++)
            {
                XmlNodeList nameArray = xmlDocument.GetElementsByTagName("nameArray"+i);
                names[i] = nameArray[0].InnerText;
            }
            save.nameArray = names.ToArray();
        }
        else
        {
            Debug.LogError("there is no save in this instance");
            for (int i = 0; i < 10; i++)
            {
                names.Add("");
                scores.Add(0);
                bpm.Add(0);
            }
        }
    }

    private Save CreatSave()
    {
        Save save = new Save();
        save.scoreArray = scores.ToArray();
        save.bpmArray = bpm.ToArray();
        save.nameArray = names.ToArray();
        return save;
    }
    public void CountScore(float value, int _bpm, bool isVictory)
    {
        victory = false;
        gameOver = false;
        if (isVictory)
            victory = true;
        else
            gameOver = true;
        placeInLeaderBoard = 1000;
        for (int i = 9; i >= 0; i--)
        {
            if(value <= scores[i] )
            {
                // if the player is to bad, dont put it in this glorious leader board. We dont like filthy pawns.
                if (i == 9)
                    break;
                else
                {
                    scores.Insert(i-1, value);
                    bpm.Insert(i-1, _bpm);
                    scores.RemoveAt(10);
                    bpm.RemoveAt(10);
                    placeInLeaderBoard = i-1;
                    break;
                }
            }
            if (i == 0)
            {
                scores.Insert(0, value);
                names.Insert(0, "aaa");
                bpm.Insert(0, _bpm);
                scores.RemoveAt(10);
                names.RemoveAt(10);
                bpm.RemoveAt(10);
                placeInLeaderBoard = 0;
            }
        }
        SaveByXML();
    }
}
