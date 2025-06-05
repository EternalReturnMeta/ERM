using System.Collections;
using UnityEngine;
using Fusion;

public class Shoichi_Skill : HeroSkill
{
    [SerializeField] private GameObject skillQChargedPrefab;
    [SerializeField] private GameObject skillQUnchargedPrefab;
    
    private HeroInput heroInput;
    [Networked] public NetworkButtons ButtonsPrevious { get; set; }
    [Networked] private int ButtonsPreviousQ { get; set; }
    [Networked] public int IsQCharged { get; set; }
    [Networked, Capacity(4)] public NetworkArray<int> CoolDownEndTick => default;
    private const float QCoolDownDuration = 3f;
    
    private HeroMovement heroMovement;
    private Shoichi_AnimationController animationController;

    private Vector3 _skillQDir;

    private bool isCasting;
    
    public override void Spawned()
    {
        heroMovement = GetComponent<HeroMovement>();
        animationController = GetComponent<Shoichi_AnimationController>();
        
        ButtonsPreviousQ = 0;
        isCasting = false;
        IsQCharged = 0;
    }

    public override void FixedUpdateNetwork()
    {
        if (!HasStateAuthority) return;
      
        if (GetInput(out heroInput))
        {
            if(heroInput.Buttons.WasPressed(ButtonsPrevious, InputButton.SkillQ))
            {
                if (Runner.Tick < CoolDownEndTick[0])
                {
                    return;
                }
                
                if (ButtonsPreviousQ == 0)
                {
                    ButtonsPreviousQ = 1;

                    if (IsQCharged == 0)
                    {
                        Skill_Q_Uncharged(heroInput.Owner);
                    }
                    else
                    {
                        Skill_Q_Charged(heroInput.Owner);
                    }
                    
                    CoolDownEndTick.Set(0, Runner.Tick + Mathf.CeilToInt(QCoolDownDuration / Runner.DeltaTime));
                }
            }
            if (heroInput.Buttons.WasReleased(ButtonsPrevious, InputButton.SkillQ))
            {
                ButtonsPreviousQ = 0;
            }
        }
        
        ButtonsPrevious = heroInput.Buttons;
    }
    
    private void Skill_Q_Uncharged(PlayerRef player)
    {
        if (isCasting)
        {
            return;
        }
        
        isCasting = true;
        animationController.RPC_Multi_Skill_Q_Uncharged();
        
        heroMovement.IsCastingSkill = true;
        
        var hitBox = Runner.Spawn(skillQUnchargedPrefab, gameObject.transform.position, Quaternion.LookRotation(gameObject.transform.forward));
        hitBox.GetComponent<Shoichi_Q_Uncharged>().Init(player);

        StartCoroutine(SkillDuration(0.3f));
    }

    private IEnumerator SkillDuration(float duration)
    {
        yield return new WaitForSeconds(duration);
        
        heroMovement.IsCastingSkill = false;
        isCasting = false;
    }
    
    private void Skill_Q_Charged(PlayerRef player)
    {
        if (isCasting)
        {
            return;
        }
        
        isCasting = true;
        animationController.RPC_Multi_Skill_Q_Charged();
        
        heroMovement.IsCastingSkill = true;

        IsQCharged = 0;
        
        var knife = Runner.Spawn(skillQChargedPrefab, gameObject.transform.position, Quaternion.LookRotation(gameObject.transform.forward));
        knife.GetComponent<Shoichi_Q_Charged>().Init(player);

        StartCoroutine(SkillDuration(0.3f));
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
