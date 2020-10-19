using System.Collections;
using System.Collections.Generic;
using UnityEngine;



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
        target = Main.Instance.player.transform;
    }

   

    // Update is called once per frame
    void Update()
    {

        transform.position = Vector3.Lerp(transform.position, target.transform.position,Random.Range(minSpeed, maxSpeed));

        if (Vector3.Distance(target.position, transform.position) <= 0.3f)
        {
            Main.Instance.specialCount++;
            Main.Instance.specialBar.value = Main.Instance.specialCount;
            Destroy(gameObject);
        }
    }
        
}
