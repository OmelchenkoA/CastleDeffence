using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AnimationControl : MonoBehaviour
{
    public Animator animator;
    public NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSpeed();
    }

	private void UpdateSpeed()
	{
        animator.SetFloat("Speed", agent.velocity.magnitude);
        
	}
}
