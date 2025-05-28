using Fusion;
using UnityEngine;

public enum InputButton
{
    SkillQ,
    SkillW,
    SkillE,
    SkillR,
    LeftClick,
    RightClick,
}

public struct HeroInput : INetworkInput
{
    public NetworkButtons Buttons;
    public Vector3 HitPosition;

    public PlayerRef Owner;
     
}