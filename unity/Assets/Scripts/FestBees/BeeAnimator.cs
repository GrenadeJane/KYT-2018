using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeAnimator : MonoBehaviour {

    Animator animator;

    // Use this for initialization
    void Start ()
    {
        animator = GetComponentInChildren<Animator>();	
	}
	
    public void Idle()
    {
        animator.SetTrigger("Idle");
    }

    public void Harvesting()
    {
        animator.SetTrigger("Harvesting");
    }

    public void Drunk()
    {
        animator.SetTrigger("Drunk");
    }

    public void Death()
    {
        animator.SetTrigger("Death");
    }
}
