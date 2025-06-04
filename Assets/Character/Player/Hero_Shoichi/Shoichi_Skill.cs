using UnityEngine;
using Fusion;

public class Shoichi_Skill : HeroSkill
{
    private HeroInput heroInput;
    [Networked] public NetworkButtons ButtonsPrevious { get; set; }
    [Networked] private int ButtonsPreviousQ { get; set; }
    [Networked] public int IsQCharged { get; private set; }

    [SerializeField] private GameObject knife;
    [SerializeField] private GameObject hitBox;
    
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
