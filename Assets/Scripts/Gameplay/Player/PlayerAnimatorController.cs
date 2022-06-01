using System;
using System.Collections;
using System.Collections.Generic;
using Gameplay.Player;
using UnityEngine;

public class PlayerAnimatorController : MonoBehaviour
{

    private const string Idle = "Idle";
    private const string Running = "Run";
    private const string Walking = "Walk";

    private PlayerController _controller;

    [SerializeField] private Animator animator;


    public void Initialize(PlayerController playerController)
    {
        _controller = playerController;
        Play(Idle);
    }

    public IEnumerator PlayDelayed(string key, float delay)
    {
        yield return new WaitForSeconds(delay);
        animator.SetTrigger(key);
    }

    public void Play(string key)
    {
        animator.SetTrigger(key);
    }



    private void Update()
    {
        if (_controller.Movement.IsMoving)
        {
            var speed = _controller.Movement.LastDirection.magnitude;

            if (speed < 0.5f)
            {
                animator.ResetTrigger(Idle);
                animator.ResetTrigger(Running);
                animator.SetTrigger(Walking);
            }

            else
            {
                animator.ResetTrigger(Idle);
                animator.ResetTrigger(Walking);
                animator.SetTrigger(Running);
            }
            
            
            animator.speed = speed<0.6f ? 1 : speed*2;
            
            
        }
        else
        {
            animator.speed = 1;
            animator.ResetTrigger(Walking);
            animator.ResetTrigger(Running);
            animator.SetTrigger(Idle);
        }
    }
}
