using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(animator.enabled)
            animator.enabled = false;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (!animator.enabled)
            animator.enabled = true;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (animator.enabled)
            animator.enabled = false;
    }
}
