using System;
using Fusion;
using UnityEngine;

public class PlayerAnimation : NetworkBehaviour
{
    
    [SerializeField] private Animator animator;
    private PlayerMovement movement;
    
    [Networked] private int MoveVelocity {get; set;}
    void Start()
    {
        movement = GetComponent<PlayerMovement>();
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
