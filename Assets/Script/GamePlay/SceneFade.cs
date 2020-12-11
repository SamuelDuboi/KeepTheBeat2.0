using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneFade : MonoBehaviour
{

  private  IEnumerator Start()
    {

        PostProcessManager.post.ActivatePostProcess((int)postProcess.Transition);
        yield return new WaitForSeconds(0.5f);
        PostProcessManager.post.DeactivatePostProcess();
        yield return new WaitForSeconds(1f);

        if (SceneManager.GetActiveScene().name == "Tuto")
        {
            SoundDisplqyTuto.Instance.canStart = true;
            
                MainTuto.Instance.positionEnd[2].GetComponent<Spawner>().ApppearAll();
                MainTuto.Instance.positionEnd[3].GetComponent<Spawner>().ApppearAll();
            
        }
        else if (SceneManager.GetActiveScene().name == "Main")
        {
            SoundDisplay.Instance.canStart = true;
            foreach (var spawn in Main.Instance.positionEnd)
            {
                spawn.GetComponent<Spawner>().ApppearAll();
            }
        }

    }
       
}
