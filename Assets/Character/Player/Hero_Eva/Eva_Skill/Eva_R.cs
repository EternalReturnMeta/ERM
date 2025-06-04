using System;
using System.Collections.Generic;
using System.Threading;
using Character.Player;
using Cysharp.Threading.Tasks;
using Fusion;
using UnityEngine;

public class Eva_R : NetworkBehaviour
{
    private PlayerRef owner;

    // private float damage = 5f;
    // private float loopTerm = 0.25f;

    private CancellationTokenSource _cts;

    private void Awake()
    {
        Utility.RefreshToken(ref _cts);
    }

    public void Init(PlayerRef player)
    {
        owner = player;
        Debug.Log($"구체의 주인 : {owner}");
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponentInParent<NetworkObject>() == null) return;
        // 주인이 맞았다면
        if (other.GetComponentInParent<NetworkObject>().InputAuthority == owner)
        {
            //Debug.Log($"구체의 오너 : {owner} || 맞은넘 : {other.GetComponentInParent<NetworkObject>().InputAuthority} ==> 내꺼니까 무시할게");
            return;
        }
        
        Debug.Log($"구체의 오너 : {owner} || 맞은넘 : {other.GetComponentInParent<NetworkObject>().InputAuthority} ==> 데미지 줄게");
     
        DamageLoop(other, _cts.Token).Forget();
        
        // IDamageProcess damageProcess = other.GetComponent<IDamageProcess>();
        // if (damageProcess != null && other.GetComponent<HeroState>().GetCurrHealth() > 0f)
        // {
        //     damageProcess.TakeDamageLoopHitStart(damage, loopTerm);
        // }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.GetComponentInParent<NetworkObject>() == null) return;
        // 주인이 맞았다면
        if (other.GetComponentInParent<NetworkObject>().InputAuthority == owner) return;
        
        // IDamageProcess damageProcess = other.GetComponent<IDamageProcess>();
        //
        // if (damageProcess != null)
        // {
        //     damageProcess.TakeDamageLoopHitStop();
        // }
        Utility.RefreshToken(ref _cts);
    }

    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        Utility.RefreshToken(ref _cts);
    }

    private async UniTaskVoid DamageLoop(Collider other, CancellationToken token)
    {
        IDamageProcess damageProcess = other.GetComponent<IDamageProcess>();
        
        while (damageProcess != null && other.GetComponent<HeroState>().GetCurrHealth() > 0f)
        {
            damageProcess.TakeDamageOneHit(5f);
            Debug.Log(other.GetComponent<HeroState>().GetCurrHealth());
            
            await UniTask.Delay(500, cancellationToken:token).SuppressCancellationThrow();
            
            if(token.IsCancellationRequested)
                break;
           
        }
    }
}
