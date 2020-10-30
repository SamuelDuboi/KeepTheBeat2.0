using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AudioHelm;
using UnityEngine.SceneManagement;
public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] private bool changing = false;

    [SerializeField] private GameObject audioClock;

    [SerializeField] private int multiplicateur = 0;
    [SerializeField] private bool isBoss;

    [Header("Tracks")] //Les audioSources
    [SerializeField] private List<GameObject> tracks = new List<GameObject>();

    // Path : Resources.LoadAll(Assets/Resources/Sounds/BPM_) avec _ = BPM selected
    [SerializeField] private AudioClip[] audioClipsSelected;

    public float getBpm = 0;

    // Start is called before the first frame update
    void Start()
    {
        audioClock = gameObject;

        getBpm = AudioHelmClock.Instance.bpm;

        foreach (Transform child in transform)
        {
            tracks.Add(child.gameObject);
        }

        SelectTracks();
    }

    private void Update()
    {
        if(SceneManager.GetActiveScene().name != "Tuto")
            isBoss = SoundDisplay.Instance.isBoss;
        else
            isBoss = SoundDisplqyTuto.Instance.isBoss;
        multiplicateur = Score.Instance.scorMultiplier;
    }

   
    public void SelectTracks()
    {
       if(getBpm <= 62)
       {
            //Load les 60 Bpm
            audioClipsSelected = Resources.LoadAll<AudioClip>("Sounds/BPM60");
            
       }
       else if(getBpm >= 63 && getBpm <= 67)
       {
            //Load les 65 Bpm
            audioClipsSelected = Resources.LoadAll<AudioClip>("Sounds/BPM65");
           
       }
        else if (getBpm >= 68 && getBpm <= 72)
        {
            //Load les 70 Bpm
            audioClipsSelected = Resources.LoadAll<AudioClip>("Sounds/BPM70");
            
        }
        else if (getBpm >= 73 && getBpm <= 77)
        {
            //Load les 75 Bpm
            audioClipsSelected = Resources.LoadAll<AudioClip>("Sounds/BPM75");
            
        }
        else if (getBpm >= 78 && getBpm <= 82)
        {
            //Load les 80 Bpm
            audioClipsSelected = Resources.LoadAll<AudioClip>("Sounds/BPM80");
            
        }
        else if (getBpm >= 83 && getBpm <= 87)
        {
            //Load les 85 Bpm
            audioClipsSelected = Resources.LoadAll<AudioClip>("Sounds/BPM85");
           
        }
        else if (getBpm >= 88 && getBpm <= 92)
        {
            Debug.Log("La");
            //Load les 90 Bpms
            audioClipsSelected = Resources.LoadAll<AudioClip>("Sounds/BPM90");
           
        }
        else if (getBpm >= 93 && getBpm <= 97)
        {
            //Load les 95 Bpm
            audioClipsSelected = Resources.LoadAll<AudioClip>("Sounds/BPM95");
            
        }
        else if (getBpm >= 98 && getBpm <= 102)
        {
            //Load les 100 Bpm
            audioClipsSelected = Resources.LoadAll<AudioClip>("Sounds/BPM100");
           
        }
        else if (getBpm >= 103 && getBpm <= 107)
        {
            //Load les 105 Bpm
            audioClipsSelected = Resources.LoadAll<AudioClip>("Sounds/BPM105");
            
        }
        else if (getBpm >= 108 && getBpm <= 112)
        {
            //Load les 110 Bpm
            audioClipsSelected = Resources.LoadAll<AudioClip>("Sounds/BPM110");
         
        }
        else if (getBpm >= 113 && getBpm <= 117)
        {
            //Load les 115 Bpm
            audioClipsSelected = Resources.LoadAll<AudioClip>("Sounds/BPM115");
      
        }
        else if (getBpm >= 118 && getBpm <= 150)
        {
            //Load les 120 Bpm
            audioClipsSelected = Resources.LoadAll<AudioClip>("Sounds/BPM120");
   
        }
        LinkToGameObject();
    }

    private void LinkToGameObject()
    {
        for (int i = 0; i < audioClipsSelected.Length; i++)
        {
            tracks[i].GetComponent<AudioSource>().clip = audioClipsSelected[i];
            tracks[i].GetComponent<AudioSource>().volume = 0f;
            tracks[i].GetComponent<AudioSource>().Play();
            
        }
        tracks[0].GetComponent<AudioSource>().volume = 1f;
    }
   
    public void StartDelay()
    {
        tracks[0].GetComponent<AudioSource>().volume = 1f;       

        for (int i = 0; i < audioClipsSelected.Length; i++)
        {
            tracks[i].GetComponent<AudioSource>().Play();
        }
    }

    public void UpdateVolume( int multiplier)
    {
        switch (multiplier)
        {
            case 1://Kick
                StartCoroutine("FadeInSound", tracks[0]);
                StartCoroutine("FadeOutSound", tracks[1]);
                break;
            case 2://Clap
                StartCoroutine("FadeInSound", tracks[1]);
                StartCoroutine("FadeOutSound", tracks[2]);
                break;
            case 3://HitHat
                StartCoroutine("FadeInSound", tracks[2]);
                StartCoroutine("FadeOutSound", tracks[3]);
                break;
            case 4://Bells
                StartCoroutine("FadeInSound", tracks[3]);
                StartCoroutine("FadeOutSound", tracks[4]);
                break;
            case 5://Synth
                StartCoroutine("FadeInSound", tracks[4]);
                StartCoroutine("FadeOutSound", tracks[5]);
                break;
            case 6://Bells 2
                StartCoroutine("FadeInSound", tracks[5]);
                StartCoroutine("FadeOutSound", tracks[6]);
                break;

            default:
                break;
        }       
          
        
            StartCoroutine("FadeOutSound", tracks[7]);
            StartCoroutine("FadeOutSound", tracks[8]);

            if (multiplicateur >= 4)
            {
                StartCoroutine("FadeInSound", tracks[4]);
            }
            else
            {
                StartCoroutine("FadeOutSound", tracks[4]);
            }
        


    }

    public void BossEntry()
    {
        StartCoroutine("FadeInSound", tracks[7]);
        StartCoroutine("FadeInSound", tracks[8]);
        StartCoroutine("FadeOutSound", tracks[4]);
    }

    IEnumerator FadeOutSound(GameObject track)
    {
        if(track.GetComponent<AudioSource>().volume != 0)
        {
            track.GetComponent<AudioSource>().volume -= 0.1f;
            yield return new WaitForSeconds(0.2f);
            StartCoroutine("FadeOutSound",track);
        }
    }

    IEnumerator FadeInSound(GameObject track)
    {
        if (track.GetComponent<AudioSource>().volume < 0.7f)
        {
            track.GetComponent<AudioSource>().volume += 0.1f;
            yield return new WaitForSeconds(0.2f);
            StartCoroutine("FadeInSound", track);
        }
    }
}
