using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MovementToHeart : MonoBehaviour
{
    public Transform target;
    public Vector3 velocity = Vector3.zero;
    
    private float speed;
    private float direction, directionStart;
  

   

    public bool isFollowing = false;


    // Start is called before the first frame update
    void Start()
    {
        target = gameObject.transform;
        if (SceneManager.GetActiveScene().name != "Tuto")
            target = Main.Instance.player.transform;
        else
            target = MainTuto.Instance.player.transform;

        directionStart = transform.position.x;
        direction = transform.position.y +3;
        speed = 0.1f;
    }



    // Update is called once per frame
    void Update()
    {
        //Vector3 sinPos = new Vector3(transform.position.x, Mathf.Sin(transform.position.x), transform.position.z); 
        transform.position = Vector3.MoveTowards(transform.position,new Vector3(directionStart, target.transform.position.y + direction, target.transform.position.z),speed);

        if (transform.position.y >= direction - 2.5f)
        {
            speed = 0.05f;
            direction = 0;
            directionStart = target.transform.position.x;
        }



        if (Vector3.Distance(target.position, transform.position) <= 0.3f)
        {
            if (SceneManager.GetActiveScene().name != "Tuto")
            {
                Main.Instance.specialCount++;
                Main.Instance.specialBarG.value = Main.Instance.specialCount;
            }
            else
            {
                MainTuto.Instance.specialBarG.value = MainTuto.Instance.specialCount;
                MainTuto.Instance.specialCount++;
            }
            Destroy(gameObject);
        }
    }
        
}
