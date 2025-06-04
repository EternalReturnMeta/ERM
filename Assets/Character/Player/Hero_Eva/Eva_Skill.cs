using System;
using System.Collections;
using Fusion;
using UnityEngine;

public class Eva_Skill : HeroSkill
{
    private HeroInput heroInput;
    [Networked] public NetworkButtons ButtonsPrevious { get; set; }
    [Networked] private int ButtonsPreviousQ { get; set; }
    [Networked] private int ButtonsPreviousR { get; set; }

    [SerializeField] private GameObject _skillQ;
    [SerializeField] private GameObject _skillR;
    private NetworkObject skillR_Dummy;
    
    private Vector3 _skillQDir {get; set;}

    private bool IsCasting;
    
    private HeroMovement heroMovement;
    private Eva_AnimationController animationController;

    private bool IsActivating_R{ get; set;}
    private Coroutine Coroutine_R;

    [Networked] private Vector3 skillR_Dir { get; set; }
    private Vector3 Skill_R_MousePosition;
    public override void Spawned()
    {
        ButtonsPreviousQ = 0;
        IsCasting = false;
        IsActivating_R = false;
        
        heroMovement = GetComponent<HeroMovement>();
        animationController = GetComponent<Eva_AnimationController>();
    }

    public override void FixedUpdateNetwork()
    {
        if (!HasStateAuthority) return;
      
        if (GetInput(out heroInput))
        {
            // =================== 스킬 Q =======================================
            if(heroInput.Buttons.WasPressed(ButtonsPrevious, InputButton.SkillQ))
            {
                if (ButtonsPreviousQ == 0)
                {
                    ButtonsPreviousQ = 1; 
                    Skill_Q(heroInput);
                }
            }
            if (heroInput.Buttons.WasReleased(ButtonsPrevious, InputButton.SkillQ))
            {
                ButtonsPreviousQ = 0;
            }
            
            // =================== 스킬 R =======================================
            if(heroInput.Buttons.WasPressed(ButtonsPrevious, InputButton.SkillR))
            {
                if (ButtonsPreviousR == 0)
                {
                    ButtonsPreviousR = 1;
                    Skill_R(heroInput);
                }
            }
            if (heroInput.Buttons.WasReleased(ButtonsPrevious, InputButton.SkillR))
            {
                ButtonsPreviousR = 0;
            }
        }

        Skill_R_MousePosition = heroInput.MousePosition;
        
        ButtonsPrevious = heroInput.Buttons;
    }
    

    private void Skill_Q(HeroInput _heroInput)
    {
        if (IsCasting) return;
        
        IsCasting = true;
        
        animationController.RPC_Multi_Skill_Q();
        
        _skillQDir = _heroInput.HitPosition_Skill - gameObject.transform.position;
        _skillQDir = new Vector3(_skillQDir.x, 0, _skillQDir.z);
        Quaternion lookRotation = Quaternion.LookRotation(_skillQDir.normalized);
        
        heroMovement.GetKcc().SetLookRotation(lookRotation, true, false);
        heroMovement.IsCastingSkill = true;
        
        StartCoroutine(Skill_Q_Coroutine(_skillQDir, _heroInput.Owner));

    }

    IEnumerator Skill_Q_Coroutine(Vector3 _skillQDir2, PlayerRef _owner)
    {
        yield return new WaitForSeconds(0.3f);
       
        var no = Runner.Spawn(_skillQ, gameObject.transform.position, Quaternion.LookRotation(_skillQDir2));
        no.GetComponent<Eva_Q>().Init(_owner);
        
        yield return new WaitForSeconds(0.3f);
        heroMovement.IsCastingSkill = false;
        IsCasting = false;
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
        // if (!IsActivating_R)
        // {
        //     var no = Runner.Spawn(_skillR, gameObject.transform.position, Quaternion.identity);
        //     IsActivating_R = true;
        //     animationController.RPC_Multi_Skill_R_Activate();
        //     heroMovement.IsCastingSkill = true;
        // }
        // else
        // {
        //     IsActivating_R = false;
        //     animationController.RPC_Multi_Skill_R_Deactivate();
        //     heroMovement.IsCastingSkill = false;
        // }
    }

    private void Skill_R(HeroInput _heroInput)
    {
        if (!IsActivating_R)
        {
            _skillQDir = _heroInput.HitPosition_Skill - gameObject.transform.position;
            _skillQDir = new Vector3(_skillQDir.x, 0, _skillQDir.z);
            Quaternion lookRotation = Quaternion.LookRotation(_skillQDir.normalized);
            
            heroMovement.GetKcc().SetLookRotation(lookRotation, true, false);
            
            var no = skillR_Dummy = Runner.Spawn(_skillR, gameObject.transform.position, Quaternion.LookRotation(_skillQDir));
            no.GetComponent<Eva_R>().Init(_heroInput.Owner);
            
            IsActivating_R = true;
            animationController.RPC_Multi_Skill_R_Activate();
            heroMovement.IsCastingSkill = true;
            Coroutine_R = StartCoroutine(Skill_R_Coroutine());
        }
        else
        {
            Runner.Despawn(skillR_Dummy);
            IsActivating_R = false;
            animationController.RPC_Multi_Skill_R_Deactivate();
            heroMovement.IsCastingSkill = false;
            StopCoroutine(Coroutine_R);
        }
    }

    IEnumerator Skill_R_Coroutine()
    {
        while (true)
        {
            skillR_Dir = (Skill_R_MousePosition - skillR_Dummy.transform.position).normalized;

            // 3. 현재 회전에서 목표 회전으로 천천히 이동
            skillR_Dummy.transform.rotation = Quaternion.Slerp(skillR_Dummy.transform.rotation, Quaternion.LookRotation(skillR_Dir),
                Runner.DeltaTime * 0.5f // 속도 조정 (5.0f 값을 조절 가능)
            );

            // 4. 다음 프레임 대기
            yield return null;
        }

    }
}
