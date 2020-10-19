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
    public GameObject visuel;

    public bool isFollowing = false;


    // Start is called before the first frame update
    void Start()
    {
        visuel.SetActive(false);
        target = gameObject.transform;
    }

    public void StartFollowing()
    {
        isFollowing = true;
        visuel.SetActive(true);
        target = Main.Instance.player.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (isFollowing)
        {
            transform.position = Vector3.SmoothDamp(transform.position, target.position, ref _velocity, Time.deltaTime * Random.Range(minSpeed, maxSpeed));
        }

        if (Vector3.Distance(target.position, transform.position) <= 0.2f)
        {
            //Ajouter Score
            Destroy(gameObject);
        }
    }
        
}
