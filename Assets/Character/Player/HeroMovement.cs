using System;
using System.Collections;
using Fusion;
using Fusion.Addons.SimpleKCC;
using UnityEngine;
using UnityEngine.AI;

public class HeroMovement : NetworkBehaviour
{
    private Camera mainCamera;
    [SerializeField] private LayerMask groundLayer; // 바닥 레이어
    [HideInInspector] public NavMeshAgent navMeshAgent;
    private SimpleKCC kcc;

    [Networked] public NetworkButtons ButtonsPrevious { get; set; }

    private HeroInput heroInput;
    private int moveCheck;
    public event Action<int> OnMove;

    public float baseSpeed;
    private NavMeshPath path;
    private Vector3 lastPos;
    
    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        path = new NavMeshPath();
        moveCheck = 0;

        kcc = GetComponentInChildren<SimpleKCC>();
    }
    
    private void Start()
    {
        mainCamera = Camera.main;
    }

    public override void Spawned()
    {   
        // 클라이언트 agent 비활성화
        if (!HasStateAuthority)
        {
            navMeshAgent.enabled = false;
        }
        // 호스트/서버 agent 업데이트 비활성화
        else
        {
            //navMeshAgent.updatePosition = false;
        }
    }

    public override void FixedUpdateNetwork() 
    {
        if (!HasStateAuthority)
            return;
        
        if (GetInput(out heroInput))
        {
            // if(heroInput.Buttons.WasPressed(ButtonsPrevious, InputButton.RightClick))
            // {
            //     ToggleProcess(true);
            // }
            // if (heroInput.Buttons.WasReleased(ButtonsPrevious, InputButton.RightClick))
            // {
            //     ToggleProcess(false);
            // }

            if (heroInput.HitPosition_RightClick != Vector3.zero)
            {
                lastPos = heroInput.HitPosition_RightClick;
            }
        }
        ButtonsPrevious = heroInput.Buttons;
        
        navMeshAgent.CalculatePath(lastPos, path);
        if (path.corners != null && path.corners.Length > 0)
        {
            Vector3 nextWaypoint;//
            if (path.corners.Length == 1)
            {
                nextWaypoint = path.corners[0];
            }
            else
            {
                nextWaypoint = path.corners[1];
            }
            var dist = Vector3.Distance(kcc.Position, nextWaypoint);
            
            if (dist <= navMeshAgent.stoppingDistance && path.corners.Length <= 2)
            {
                 OnMove?.Invoke(0);
                 return;
            }
            
            OnMove?.Invoke(1);
            
            var speed = baseSpeed * (60f / Runner.TickRate);
            var direction = (nextWaypoint - kcc.Position).normalized;
            kcc.Move(direction * speed);
            
            float targetYaw = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float currentPitch = kcc.GetLookRotation(true, false).x;
            kcc.SetLookRotation(currentPitch, targetYaw);
            
        }
    }

    public override void Render()
    {
        if (HasStateAuthority)
        {
            if (navMeshAgent)
            {
                if (navMeshAgent.remainingDistance < 0.1f)
                {
                    if (moveCheck == 1)
                    {
                        //OnMove?.Invoke(0);
                        moveCheck = 0;
                    }
                }
            }
        }
    }
    
    // private void ToggleProcess(bool isOn)
    // {
    //     if (isOn)
    //     {
    //         StartCoroutine(SetPositionProcess());
    //     }
    //     else
    //     {
    //         StopAllCoroutines();
    //         moveCheck = 1;
    //     }
    // }
    //
    // private IEnumerator SetPositionProcess()
    // {
    //     while (true)
    //     {
    //         OnMove?.Invoke(1);
    //         
    //         navMeshAgent.isStopped = false;
    //         //navMeshAgent.SetDestination(heroInput.HitPosition);
    //         
    //         yield return new WaitForSeconds(0.15f);
    //     }
    // }
    
}