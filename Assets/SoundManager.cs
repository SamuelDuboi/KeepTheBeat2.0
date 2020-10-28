using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AudioHelm;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private bool changing = false;

    [SerializeField] private GameObject audioClock;

    [SerializeField] private int multiplicateur = 0;
    [SerializeField] private bool isBoss = false;

    [Header("Tracks")] //Les audioSources
    [SerializeField] private List<GameObject> tracks = new List<GameObject>();

    // Path : Resources.LoadAll(Assets/Resources/Sounds/BPM_) avec _ = BPM selected
    [SerializeField] private AudioClip[] audioClipsSelected;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in transform)
        {
            tracks.Add(child.gameObject);
        }

        SelectTracks();
    }

    private void Update()
    {
        //A changer, faire en sorte d'appeler UpdateVolume a chaque monté ou perte de Modifieur.

        if (changing)
        {
            changing = false;
            UpdateVolume();
        }
    }

   
    public void SelectTracks()
    {
       if(audioClock.GetComponent<AudioHelmClock>().bpm  <= 62)
       {
            //Load les 60 Bpm
            audioClipsSelected = Resources.LoadAll<AudioClip>("Sounds/BPM60");
            Debug.Log("La");
            
       }
       else if(audioClock.GetComponent<AudioHelmClock>().bpm >= 63 && audioClock.GetComponent<AudioHelm.AudioHelmClock>().bpm <= 67)
       {
            //Load les 65 Bpm
            audioClipsSelected = Resources.LoadAll<AudioClip>("Sounds/BPM65");
           
       }
        else if (audioClock.GetComponent<AudioHelm.AudioHelmClock>().bpm >= 68 && audioClock.GetComponent<AudioHelm.AudioHelmClock>().bpm <= 72)
        {
            //Load les 70 Bpm
            audioClipsSelected = Resources.LoadAll<AudioClip>("Sounds/BPM70");
            
        }
        else if (audioClock.GetComponent<AudioHelmClock>().bpm >= 73 && audioClock.GetComponent<AudioHelm.AudioHelmClock>().bpm <= 77)
        {
            //Load les 75 Bpm
            audioClipsSelected = Resources.LoadAll<AudioClip>("Sounds/BPM75");
            
        }
        else if (audioClock.GetComponent<AudioHelmClock>().bpm >= 78 && audioClock.GetComponent<AudioHelm.AudioHelmClock>().bpm <= 82)
        {
            //Load les 80 Bpm
            audioClipsSelected = Resources.LoadAll<AudioClip>("Sounds/BPM80");
            
        }
        else if (audioClock.GetComponent<AudioHelmClock>().bpm >= 83 && audioClock.GetComponent<AudioHelm.AudioHelmClock>().bpm <= 87)
        {
            //Load les 85 Bpm
            audioClipsSelected = Resources.LoadAll<AudioClip>("Sounds/BPM85");
           
        }
        else if (audioClock.GetComponent<AudioHelmClock>().bpm >= 88 && audioClock.GetComponent<AudioHelm.AudioHelmClock>().bpm <= 92)
        {
            //Load les 90 Bpm
            audioClipsSelected = Resources.LoadAll<AudioClip>("Sounds/BPM90");
           
        }
        else if (audioClock.GetComponent<AudioHelmClock>().bpm >= 93 && audioClock.GetComponent<AudioHelm.AudioHelmClock>().bpm <= 97)
        {
            //Load les 95 Bpm
            audioClipsSelected = Resources.LoadAll<AudioClip>("Sounds/BPM95");
            
        }
        else if (audioClock.GetComponent<AudioHelmClock>().bpm >= 98 && audioClock.GetComponent<AudioHelm.AudioHelmClock>().bpm <= 102)
        {
            //Load les 100 Bpm
            audioClipsSelected = Resources.LoadAll<AudioClip>("Sounds/BPM100");
           
        }
        else if (audioClock.GetComponent<AudioHelmClock>().bpm >= 103 && audioClock.GetComponent<AudioHelm.AudioHelmClock>().bpm <= 107)
        {
            //Load les 105 Bpm
            audioClipsSelected = Resources.LoadAll<AudioClip>("Sounds/BPM105");
            
        }
        else if (audioClock.GetComponent<AudioHelmClock>().bpm >= 108 && audioClock.GetComponent<AudioHelm.AudioHelmClock>().bpm <= 112)
        {
            //Load les 110 Bpm
            audioClipsSelected = Resources.LoadAll<AudioClip>("Sounds/BPM110");
         
        }
        else if (audioClock.GetComponent<AudioHelmClock>().bpm >= 113 && audioClock.GetComponent<AudioHelm.AudioHelmClock>().bpm <= 117)
        {
            //Load les 115 Bpm
            audioClipsSelected = Resources.LoadAll<AudioClip>("Sounds/BPM115");
      
        }
        else if (audioClock.GetComponent<AudioHelmClock>().bpm >= 118 && audioClock.GetComponent<AudioHelm.AudioHelmClock>().bpm <= 150)
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
        }
        StartCoroutine("StartDelay");
    }

    IEnumerator StartDelay()
    {
        tracks[0].GetComponent<AudioSource>().volume = 1f;
        yield return new WaitForSecondsRealtime(1f);
       

        for (int i = 0; i < audioClipsSelected.Length; i++)
        {
            tracks[i].GetComponent<AudioSource>().Play();
        }
    }

    private void UpdateVolume()
    {
        //Kick
        if(multiplicateur == 0)
        {
            StartCoroutine("FadeInSound", tracks[0]);
            StartCoroutine("FadeOutSound", tracks[1]);
        }
        if (multiplicateur == 1) //Clap
        {
            StartCoroutine("FadeInSound", tracks[1]);
            StartCoroutine("FadeOutSound", tracks[2]);
        }
        if (multiplicateur == 2) //HitHat
        {
            StartCoroutine("FadeInSound", tracks[2]);
            StartCoroutine("FadeOutSound", tracks[3]);
        }
        if (multiplicateur == 3) //Bells
        {
            StartCoroutine("FadeInSound", tracks[3]);
            StartCoroutine("FadeOutSound", tracks[4]);
        }
        if (multiplicateur == 4) //Synth
        {
            StartCoroutine("FadeInSound", tracks[4]);
            StartCoroutine("FadeOutSound", tracks[5]);
        }
        if (multiplicateur == 5) //Bells 2
        {
            StartCoroutine("FadeInSound", tracks[5]);
            StartCoroutine("FadeOutSound", tracks[6]);
        }

        if (isBoss)
        {
            StartCoroutine("FadeInSound", tracks[7]);
            StartCoroutine("FadeInSound", tracks[8]);
            StartCoroutine("FadeOutSound", tracks[4]);

            
        }
        else
        {
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
