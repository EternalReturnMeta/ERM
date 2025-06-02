using System;
using Fusion;
using UnityEngine;

public class Eva_AnimationController : NetworkBehaviour
{
    
    [SerializeField] private Animator animator;
    private HeroMovement movement;
    
    [Networked] private int MoveVelocity {get; set;}
   
    public override void Spawned()
    {
        movement = GetComponent<HeroMovement>();
        movement.OnMoveVelocityChanged += OnChangedVelocity;

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

    private void OnChangedVelocity(int v)
    {
        MoveVelocity = v;
    }
    
    [Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    public void RPC_Multi_Skill_Q()
    {
        animator.SetTrigger("tSkill01");
    }
    
    [Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    public void RPC_Multi_Skill_R_Activate()
    {
        animator.SetTrigger("tSkill04");
        animator.SetBool("bSkill04", true);
    }
    
    [Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    public void RPC_Multi_Skill_R_Deactivate()
    {
        animator.SetBool("bSkill04", false);
    }
    [Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    public void RPC_DeadProcess()
    {
        if (animator)
        {
            MoveVelocity = 0;
            animator.SetTrigger("IsDead");
        }
    }
}
