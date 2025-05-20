using Fusion;
using UnityEngine;
using UnityEngine.AI;

public class NewPlayerMovement : NetworkBehaviour
{
    public Camera mainCamera; // 카메라 참조
    private NavMeshAgent navMeshAgent; // 플레이어의 NavMeshAgent
    public LayerMask groundLayer; // 바닥 레이어
    
    [Networked] public Vector3 TargetPosition { get; set; }
    
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        mainCamera = Camera.main;
        // LocalPlayer가 아니라면 네비게이션 비활성화 (서버 또는 다른 클라이언트 제어 방지)
        //navMeshAgent.enabled = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (Object.HasInputAuthority && Input.GetMouseButtonDown(1))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
            {
                SetTargetPosition(hit.point);
            }
        }

        // 목표 지점으로 이동
        if (navMeshAgent.remainingDistance < 0.1f)
        {
            navMeshAgent.isStopped = true;
        }

    }

    public void SetTargetPosition(Vector3 target)
    {
        RPC_ServerSetTargetPosition(target);
    }
    
    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority, HostMode = RpcHostMode.SourceIsHostPlayer)]
    public void RPC_ServerSetTargetPosition(Vector3 target, RpcInfo rpcInfo = default)
    {
        RPC_MultiSetTargetPosition(target);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    public void RPC_MultiSetTargetPosition(Vector3 target)
    {
        // 네트워크 상 모든 클라이언트에 목표 위치 동기화
        TargetPosition = target;
    
        //로컬 NavMeshAgent를 사용해 이동 처리
        navMeshAgent.SetDestination(target);
        navMeshAgent.isStopped = false;
    }
    
    public override void FixedUpdateNetwork()
    {
        // 서버나 다른 클라이언트에서도 동기화된 TargetPosition 기반으로 NavMesh 이동 처리
        //if (!IsOwner && navMeshAgent.enabled)
        //{
            //navMeshAgent.SetDestination(TargetPosition);
        //}
    }

}
