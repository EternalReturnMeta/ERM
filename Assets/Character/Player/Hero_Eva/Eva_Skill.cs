using System;
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
            Skill_Q();
        }
    }

    protected override void Skill_Q()
    {
        Debug.Log("Q");
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
