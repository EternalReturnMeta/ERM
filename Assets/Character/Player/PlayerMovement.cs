using System.Collections;
using Fusion;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : NetworkBehaviour
{
    public Camera mainCamera; // 카메라 참조
    public LayerMask groundLayer; // 바닥 레이어
    [HideInInspector] public NavMeshAgent navMeshAgent;
    
    private Vector3 TargetPosition { get; set; }
    
    private Coroutine MoveCoroutine;
    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }
    
    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (!HasInputAuthority) return;

        if (Input.GetMouseButtonDown(1))
        {
            MoveCoroutine = StartCoroutine(SetPosition());
        }
        
        if (Input.GetMouseButtonUp(1))
        {
            StopCoroutine(MoveCoroutine);
        }
        
        if (navMeshAgent.remainingDistance < 0.1f)
        {
            navMeshAgent.isStopped = true;
        }

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
            
            yield return new WaitForSeconds(0.15f);
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
        TargetPosition = target;
    
        navMeshAgent.SetDestination(target);
        navMeshAgent.isStopped = false;
    }

}
