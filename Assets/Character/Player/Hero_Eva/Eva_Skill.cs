using System;
using System.Collections;
using Fusion;
using UnityEngine;

public class Eva_Skill : HeroSkill
{
    private HeroInput heroInput;
    [Networked] public NetworkButtons ButtonsPrevious { get; set; }
    [Networked] private int ButtonsPreviousQ { get; set; }

    [SerializeField] private GameObject _skillQ;
    
    private Vector3 _skillQDir {get; set;}

    private bool IsCasting;
    public override void Spawned()
    {
        ButtonsPreviousQ = 0;
        IsCasting = false;
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
                    Skill_QQ(heroInput);
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
    }

    private void Skill_QQ(HeroInput _heroInput)
    {
        if (IsCasting) return;
        
        IsCasting = true;
        
        GetComponent<HeroAnimation>().RPC_Multi_Skill_Q();
        
        _skillQDir = _heroInput.HitPosition_Skill - gameObject.transform.position;
        _skillQDir = new Vector3(_skillQDir.x, 0, _skillQDir.z);
        Quaternion lookRotation = Quaternion.LookRotation(_skillQDir.normalized);
        
        GetComponent<HeroMovement>().GetKcc().SetLookRotation(lookRotation, true, false);
        GetComponent<HeroMovement>().IsCastingSkill = true;
        
        StartCoroutine(Skill_Q_Coroutine(_skillQDir, _heroInput.Owner));

    }

    IEnumerator Skill_Q_Coroutine(Vector3 _skillQDir2, PlayerRef _owner)
    {
        yield return new WaitForSeconds(0.4f);
       
        var no = Runner.Spawn(_skillQ, gameObject.transform.position, Quaternion.LookRotation(_skillQDir2));
        no.GetComponent<Eva_Q>().Init(_owner);
        
        GetComponent<HeroMovement>().IsCastingSkill = false;
        IsCasting = false;
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


    private void CastingSkill()
    {
        
    }
}
