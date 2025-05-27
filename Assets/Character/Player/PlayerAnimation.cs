using System;
using Fusion;
using UnityEngine;

public class PlayerAnimation : NetworkBehaviour
{
    
    [SerializeField] private Animator animator;
    [SerializeField] private NetworkMecanimAnimator mecanimAnimator;
    private PlayerMovement movement;
    
    void Start()
    {
        movement = GetComponent<PlayerMovement>();
        mecanimAnimator = GetComponent<NetworkMecanimAnimator>();
    }

    public override void FixedUpdateNetwork()
    {
       
    }

    private void Update()
    {
        if( !HasInputAuthority ) return;
        
    }
    
    public override void Render()
    {
        if (animator)
        {
            if (movement)
            {
                animator.SetFloat("MoveSpeed", movement.navMeshAgent.velocity.magnitude);
            }
        }
    }
}
