using Fusion;
using UnityEngine;

public class Shoichi_Q_Uncharged : NetworkBehaviour
{
    private PlayerRef owner;
    
    public void Init(PlayerRef player)
    {
        owner = player;
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
     
        IDamageable damageable = other.GetComponent<IDamageable>();
        if (damageable != null && other.GetComponent<HeroState>().GetCurrHealth() > 0f)
        {
            damageable.TakeDamage(10);
        }
    }
}
