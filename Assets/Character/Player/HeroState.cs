using Fusion;
using UnityEngine;

public class HeroState : NetworkBehaviour, IDamageable
{
    [Networked] [field:SerializeField]
    private float CurrHealth {get; set;}
    
    [Networked] public float MaxHealth {get; set;}
    
    void Start()
    {
        MaxHealth = 100;
        CurrHealth = MaxHealth;
    }

    void Update()
    {
        
    }

    public void TakeDamage(float damage)
    {
        
        CurrHealth -= damage;
        Debug.Log($"TakeDamage : {GetComponentInParent<NetworkObject>().InputAuthority} ==> 현재 피 {CurrHealth}");
    }
}
