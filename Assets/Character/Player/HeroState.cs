using Fusion;
using UnityEngine;
using UnityEngine.UI;

public class HeroState : NetworkBehaviour, IDamageable
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

    public void TakeDamage(float damage)
    {
        CurrHealth -= damage;
        Debug.Log($"TakeDamage : {GetComponentInParent<NetworkObject>().InputAuthority} ==> 현재 피 {CurrHealth}");

        if (CurrHealth <= 0)
        {
            DeadProcess();
        }
    }

    private void DeadProcess()
    {
        Debug.Log($"{gameObject.GetComponentInParent<NetworkObject>().InputAuthority} : Dead");
    }
}
