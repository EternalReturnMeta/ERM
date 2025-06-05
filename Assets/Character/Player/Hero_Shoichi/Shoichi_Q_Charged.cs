using UnityEngine;
using Character.Player;
using Fusion;

public class Shoichi_Q_Charged : NetworkBehaviour
{
    [SerializeField] private GameObject effectPrefab;
    [Networked] private TickTimer life { get; set; }
    
    private PlayerRef owner;

    public override void Spawned()
    {
        life = TickTimer.CreateFromSeconds(Runner, 1f);
    }

    public override void FixedUpdateNetwork()
    {
        if (life.Expired(Runner))
        {
            if (Runner.TryGetPlayerObject(owner, out NetworkObject networkObject))
            {
                var skillState = networkObject.GetComponentInChildren<Shoichi_Skill>();

                if (skillState.IsQCharged == 1)
                {
                    networkObject.GetComponentInChildren<Shoichi_AnimationController>().RPC_Q_Charged();
                }
            }
            
            Runner.Despawn(Object);
        }
        else if (life.RemainingTime(Runner) <= 0.8f)
        {
            var boxCollider = GetComponent<BoxCollider>();
            boxCollider.enabled = true;
            effectPrefab.SetActive(true);
        }
    }

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
        
        if (Runner.TryGetPlayerObject(owner, out NetworkObject networkObject))
        {
            var skillState = networkObject.GetComponentInChildren<Shoichi_Skill>();

            if (skillState.IsQCharged == 0)
            {
                skillState.IsQCharged = 1;
            }
        }
     
        IDamageProcess damageProcess = other.GetComponent<IDamageProcess>();
        if (damageProcess != null && other.GetComponent<HeroState>().GetCurrHealth() > 0f)
        {
            damageProcess.OnTakeDamage(10);
        }
    }
}
