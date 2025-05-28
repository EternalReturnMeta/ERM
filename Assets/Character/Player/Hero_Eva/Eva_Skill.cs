using System;
using Fusion;
using UnityEngine;

public class Eva_Skill : HeroSkill
{
     [SerializeField] private Animator _animator;
     
     private HeroInput heroInput;
     [Networked] public NetworkButtons ButtonsPrevious { get; set; }

     [SerializeField] private GameObject _skillQ;
    [Networked] private int ButtonsPreviousQ { get; set; }
    
    private Vector3 _skillQDir {get; set;}
    

    public override void Spawned()
    {
        ButtonsPreviousQ = 0;
    }

    public override void FixedUpdateNetwork()
    {
        if (!HasStateAuthority) return;
      
        if (GetInput(out heroInput))
        {
            if(heroInput.Buttons.WasPressed(ButtonsPrevious, InputButton.SkillQ))
            {
                if (ButtonsPreviousQ == 0)
                {
                    ButtonsPreviousQ = 1;  
                    Skill_Q();
                }
            }
            if (heroInput.Buttons.WasReleased(ButtonsPrevious, InputButton.SkillQ))
            {
                ButtonsPreviousQ = 0;
            }
        }
        
        ButtonsPrevious = heroInput.Buttons;
    }
    
    protected override void Skill_Q()
    {
        RPC_Multi_Skill_Q();
        
        _skillQDir = heroInput.HitPosition - gameObject.transform.position;
        _skillQDir = new Vector3(_skillQDir.x, 0, _skillQDir.z);
        
        var no = Runner.Spawn(_skillQ, gameObject.transform.position, Quaternion.LookRotation(_skillQDir));
        no.GetComponent<Eva_Q>().Init(heroInput.Owner);
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
