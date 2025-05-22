using System.Collections;
using Fusion;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : NetworkBehaviour
{
    public Camera mainCamera; // 카메라 참조
    public LayerMask groundLayer; // 바닥 레이어
    [HideInInspector] public NavMeshAgent navMeshAgent; // 플레이어의 NavMeshAgent
    
    private Vector3 TargetPosition { get; set; }
    
    private Coroutine TempCoroutine;
    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }
    
    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (!HasInputAuthority) return;

        if (Input.GetMouseButtonDown(1))
        {
            TempCoroutine = StartCoroutine(SetPosition());
        }
        
        if (Input.GetMouseButtonUp(1))
        {
            StopCoroutine(TempCoroutine);
        }
        
        // 목표 지점으로 이동
        if (navMeshAgent.remainingDistance < 0.1f)
        {
            navMeshAgent.isStopped = true;
        }

    }

    public void SetTargetPosition(Vector3 target)
    {
        //StartCoroutine(SetPosition(target));
    }

    IEnumerator SetPosition()
    {
        while (true)
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
            {
                RPC_ServerSetTargetPosition(hit.point);
            }
            
            yield return new WaitForSeconds(0.1f);
        }
        
    }
    
    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority, HostMode = RpcHostMode.SourceIsHostPlayer)]
    public void RPC_ServerSetTargetPosition(Vector3 target)
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
