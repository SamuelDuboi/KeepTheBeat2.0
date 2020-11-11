using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using AudioHelm;

public class AddBpmBonus : MonoBehaviour
{
    public TextMeshProUGUI score;
    public TextMeshProUGUI bpmBonus;
    private bool canMove;
    
    // Start is called before the first frame update
    IEnumerator Start()
    {
        score.text = "score : " + Score.score.ToString();

        bpmBonus.text = "bpm bonnus : " + (AudioHelmClock.Instance.bpm * 100000).ToString();
        yield return new WaitForSeconds(3f);
        canMove = true;
    }

    private void Update()
    {
        if (canMove && bpmBonus.transform.localPosition.y < -33)
        {
            bpmBonus.transform.localPosition += Vector3.up*0.5f;
            bpmBonus.color -= new Color(0, 0, 0, 0.05f);
        }
        else if(canMove && bpmBonus.transform.localPosition.y >= -33)
        {
            score.text = "score : " + (Score.score + AudioHelmClock.Instance.bpm * 100000).ToString();
            canMove = false;
        }
    }
}
