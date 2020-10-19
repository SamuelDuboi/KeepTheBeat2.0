using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementToHeart : MonoBehaviour
{
    public Transform target;
    public Vector3 _velocity = Vector3.zero;
    [Range(0f,5f)]
    public float minSpeed;
    [Range(10f, 20f)]
    public float maxSpeed;

    public bool isFollowing = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void StartFollowing()
    {
        isFollowing = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isFollowing)
        {
            transform.position = Vector3.SmoothDamp(transform.position, target.position, ref _velocity, Time.deltaTime * Random.Range(minSpeed, maxSpeed));
        }
    }
        
}
