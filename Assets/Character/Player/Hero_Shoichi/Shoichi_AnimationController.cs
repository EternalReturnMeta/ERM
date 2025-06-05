using Fusion;
using UnityEngine;

public class Shoichi_AnimationController : HeroAnimationController
{
    [SerializeField] private Animator briefCaseAnimator;
    
    [Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    public void RPC_Multi_Skill_Q_Uncharged()
    {
        animator.SetBool("IsQCharged", false);
        animator.SetTrigger("tSkill01");
    }
    
    [Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    public void RPC_Multi_Skill_Q_Charged()
    {
        animator.SetBool("IsQCharged", true);
        animator.SetTrigger("tSkill01");
    }
    
    [Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    public void RPC_Q_Charged()
    {
        briefCaseAnimator.SetTrigger("IsQActivated");
    }
}
