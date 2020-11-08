using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialFullParticuleBehaviour : MonoBehaviour
{
    public GameObject endPosition;

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(0, 3, 0); 

        if(transform.position.y >= endPosition.transform.position.y)
        {
            animator.SetTrigger("Vanish");
            Destroy(gameObject);
        }
    }

   

}
