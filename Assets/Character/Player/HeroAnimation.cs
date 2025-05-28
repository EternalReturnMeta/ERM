using System;
using Fusion;
using UnityEngine;

public class HeroAnimation : NetworkBehaviour
{
    
    [SerializeField] private Animator animator;
    private HeroMovement movement;
    
    [Networked] private int MoveVelocity {get; set;}
   
    public override void Spawned()
    {
        movement = GetComponent<HeroMovement>();
        movement.OnMove += OnChangeMoveVelocity;

        MoveVelocity = 0;
    }

    public override void Render()
    {
        if (animator)
        {
            if (movement)
            {
                animator.SetFloat("MoveSpeed", MoveVelocity);
            }
        }
    }

    private void OnChangeMoveVelocity(int v)
    {
        MoveVelocity = v;
    }
}
