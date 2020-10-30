using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MovementToHeart : MonoBehaviour
{
    public Transform target;
    public Vector3 velocity = Vector3.zero;
    [Range(0f,5f)]
    public float minSpeed;
    [Range(0f, 5f)]
    public float maxSpeed;
    

    public bool isFollowing = false;


    // Start is called before the first frame update
    void Start()
    {
        target = gameObject.transform;
        if (SceneManager.GetActiveScene().name != "Tuto")
            target = Main.Instance.player.transform;
        else
            target = MainTuto.Instance.player.transform;
    }

   

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, target.transform.position,Random.Range(minSpeed, maxSpeed));

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
