using System.Collections;
using Character.Player;
using Fusion;
using Fusion.Addons.KCC;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class HeroState : NetworkBehaviour, IDamageProcess
{
    [Networked] [field:SerializeField]
    private float CurrHealth {get; set;}
    
    [Networked] [field:SerializeField]
    private float MaxHealth {get; set;}
    
    public override void Spawned()
    {
        MaxHealth = 100;
        CurrHealth = MaxHealth;
    }

    public float GetCurrHealth()
    {
        return CurrHealth;
    }

    public void OnTakeDamage(float damage)
    {
        CurrHealth -= damage;
        Debug.Log($"TakeDamage : {GetComponentInParent<NetworkObject>().InputAuthority} ==> 현재 피 {CurrHealth}");

        if (CurrHealth <= 0)
        {
            var navMeshAgent = GetComponent<NavMeshAgent>();
            //navMeshAgent.isStopped = true;
            var heroMovement = GetComponent<HeroMovement>();
            heroMovement.IsDeath = true;
            
            GetComponent<Eva_AnimationController>().RPC_DeadProcess();
        }
    }
}
