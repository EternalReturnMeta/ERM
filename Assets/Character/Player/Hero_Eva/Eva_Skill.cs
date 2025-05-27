using System;
using Fusion;
using UnityEngine;

public class Eva_Skill : PlayerBase
{
     [SerializeField] private Animator _animator;
     
     private HeroInput heroInput;
     [Networked] public NetworkButtons ButtonsPrevious { get; set; }

     [SerializeField] private GameObject _skillQ;
    [Networked] private int ButtonsPreviousQ { get; set; }
    private Quaternion lookQuaternion{ get; set;}
    
    private Vector3 _skillQDir {get; set;}
    private void Start()
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
                    Debug.Log($"QQQQQQQQQQ", gameObject);
                    RPC_Multi_Skill_Q();
                    
                    // var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    // if (Physics.Raycast(ray, out var hit, Mathf.Infinity, LayerMask.GetMask("Ground")))
                    // {
                    //     _skillQDir = hit.point - gameObject.transform.position;
                    //    
                    //     //_skillQPosition.Normalize();
                    // }
                    _skillQDir = heroInput.HitPosition - gameObject.transform.position;
                    _skillQDir = new Vector3(_skillQDir.x, 0, _skillQDir.z);
                    //lookQuaternion = Quaternion.LookRotation(_skillQDir);
                    Debug.Log($"_skillQPosition : {_skillQDir}", gameObject);
                    Runner.Spawn(_skillQ, gameObject.transform.position, Quaternion.LookRotation(_skillQDir));
                }
            }
            if (heroInput.Buttons.WasReleased(ButtonsPrevious, InputButton.SkillQ))
            {
                ButtonsPreviousQ = 0;
            }
        }
        
        ButtonsPrevious = heroInput.Buttons;
    }

    public override void Render()
    {
        
    }
    
   
    [Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    private void RPC_Multi_Skill_Q()
    {
        
        _animator.SetTrigger("tSkill01");
    }
    
    protected override void Skill_Q()
    {
        
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
