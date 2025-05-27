using System;
using Fusion;
using UnityEngine;

public class Eva_Skill : PlayerBase
{
     [SerializeField] private Animator _animator;
     

    private void Update()
    {
        
        
    }

    public override void Render()
    {
        if( !HasInputAuthority ) return;
        
        if (Input.GetKeyDown(KeyCode.Q))
        { 
            //Skill_Q();
            //QQ();
        }
    }

    public void QQ()
    {
        _animator.SetTrigger("tSkill01");

    }
    protected override void Skill_Q()
    {
        Debug.Log("Q");
        RPC_Server_Skill_Q();
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority, HostMode = RpcHostMode.SourceIsHostPlayer)]
    private void RPC_Server_Skill_Q()
    {
        if (Runner.IsServer)
        {
            Debug.Log("Server");
        }
        RPC_Multi_Skill_Q();
    }
    
    [Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    private void RPC_Multi_Skill_Q()
    {
        _animator.SetTrigger("tSkill01");
    }
    
    protected override void Skill_W()
    {
        
    }

    protected override void Skill_E()
    {
        
    }

    protected override void Skill_R()
    {
        
    }
    
}
