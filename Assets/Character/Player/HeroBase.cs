using Fusion;
using UnityEngine;

public abstract class HeroBase : NetworkBehaviour
{
    protected abstract void Skill_Q();

    protected abstract void Skill_W();

    protected abstract void Skill_E();

    protected abstract void Skill_R();

}
