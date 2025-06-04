using Fusion;
using UnityEngine;

public class Shoichi_AnimationController : NetworkBehaviour
{
    [SerializeField] private Animator animator;
    private HeroMovement movement;
    
    [Networked] public int MoveVelocity { get; private set; }

    public override void Spawned()
    {
        movement = GetComponent<HeroMovement>();
        movement.OnMoveVelocityChanged += OnChangeVelocity;
    }

    public override void Render()
    {
        if (animator)
        {
            if (!movement)
            {
                return;
            }
            
            animator.SetFloat("MoveSpeed", MoveVelocity);
        }
    }

    private void OnChangeVelocity(int v)
    {
        MoveVelocity = v;
    }
    
    [Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    public void RPC_Multi_Skill_Q()
    {
        animator.SetTrigger("tSkill01");
    }
}
