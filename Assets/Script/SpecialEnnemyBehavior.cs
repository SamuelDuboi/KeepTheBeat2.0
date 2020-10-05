using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialEnnemyBehavior : EnnemyBehavior
{
    private void Start()
    {
        SoundDisplay.Instance.specialEnnemy = gameObject;

    }
    public override void Move()
    {
        switch (cpt)
        {
            case 0:
                transform.position = positions[1];
                transform.localScale = transform.localScale / 1.2f;
                break;
            case 1:
                transform.position = positions[3];
                transform.localScale = transform.localScale / 1.6f;
                break;
            case 2:
                transform.position = positions[5];
                transform.localScale = transform.localScale / 2.7f;
                break;
                
            default:
                Score.Instance.ModifierDown();
                Destroy(gameObject);
                break;
        }
        cpt++;
           
        
    }



}
