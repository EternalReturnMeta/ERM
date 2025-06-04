using System;
using Character.Player;
using Fusion;
using UnityEngine;

public class Eva_Q : NetworkBehaviour
{
    [Networked] private TickTimer life { get; set; }

    private PlayerRef owner;
    
    public void Init(PlayerRef player)
    {
        owner = player;
        Debug.Log($"구체의 주인 : {owner}");
    }
    public override void Spawned()
    {
        life = TickTimer.CreateFromSeconds(Runner, 5.0f);
    }

    public override void FixedUpdateNetwork()
    {
        if(life.Expired(Runner))
            Runner.Despawn(Object);
        else
            transform.position += 30 * transform.forward * Runner.DeltaTime;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponentInParent<NetworkObject>() == null) return;
        // 주인이 맞았다면
        if (other.GetComponentInParent<NetworkObject>().InputAuthority == owner)
        {
            Debug.Log($"구체의 오너 : {owner} || 맞은넘 : {other.GetComponentInParent<NetworkObject>().InputAuthority} ==> 내꺼니까 무시할게");
            
            return;
        }
        
        Debug.Log($"구체의 오너 : {owner} || 맞은넘 : {other.GetComponentInParent<NetworkObject>().InputAuthority} ==> 데미지 줄게");
     
        IDamageProcess damageProcess = other.GetComponent<IDamageProcess>();
        if (damageProcess != null && other.GetComponent<HeroState>().GetCurrHealth() > 0f)
        {
            damageProcess.OnTakeDamage(10);
        }
        
    }

}
