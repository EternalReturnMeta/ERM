using System.Collections;
using UnityEngine;
using Fusion;

public class Shoichi_Skill : HeroSkill
{
    [SerializeField] private GameObject knifePrefab;
    [SerializeField] private GameObject hitBoxPrefab;
    
    private HeroInput heroInput;
    [Networked] public NetworkButtons ButtonsPrevious { get; set; }
    [Networked] private int ButtonsPreviousQ { get; set; }
    [Networked] public int IsQCharged { get; set; }
    [Networked, Capacity(4)] public NetworkArray<int> CoolDownEndTick => default;
    private const float QCoolDownDuration = 8f;
    
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
                    Skill_Q_Uncharged(heroInput.Owner);
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
        animationController.RPC_Multi_Skill_Q();
        heroMovement.IsCastingSkill = true;
        StartCoroutine(SpawnHitBox(player));
    }

    private IEnumerator SpawnHitBox(PlayerRef player)
    {
        for (int i = 0; i < 8; i++)
        {
            yield return null;
        }
        
        var hitBox = Runner.Spawn(hitBoxPrefab, gameObject.transform.forward, Quaternion.identity);
        hitBox.GetComponent<Shoichi_Q_Uncharged>().Init(player);
        hitBox.GetComponent<BoxCollider>().size = new Vector3(1f, 1f, 3f);
        hitBox.GetComponent<BoxCollider>().center = new Vector3(0f, 0.5f, 0.5f);
        
        heroMovement.IsCastingSkill = false;
        isCasting = false;
        
        for (int i = 0; i < 8; i++)
        {
            yield return null;
        }
        
        Runner.Despawn(hitBox);
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
