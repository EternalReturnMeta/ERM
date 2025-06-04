using System;
using System.Collections;
using Fusion;
using Fusion.Addons.SimpleKCC;
using UnityEngine;
using UnityEngine.AI;

public class HeroMovement : NetworkBehaviour
{
    [SerializeField] private LayerMask groundLayer; // 바닥 레이어
    [HideInInspector] public NavMeshAgent navMeshAgent;
    private SimpleKCC kcc;

    [Networked] public NetworkButtons ButtonsPrevious { get; set; }

    private HeroInput heroInput;
    public event Action<int> OnMoveVelocityChanged;

    public float baseSpeed;
    private NavMeshPath path;
    private Vector3 lastPos;
    
    public bool IsCastingSkill { get; set; }
    public bool IsDeath { get; set; }
    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        path = new NavMeshPath();

        kcc = GetComponentInChildren<SimpleKCC>();
        IsCastingSkill = false;
        IsDeath = false;
    }

    public SimpleKCC GetKcc()
    {
        return kcc;
    }
    
    public override void Spawned()
    {   
        // 클라이언트 agent 비활성화
        if (!HasStateAuthority)
        {
            navMeshAgent.enabled = false;
        }
    }

    public override void FixedUpdateNetwork() 
    {
        if (!HasStateAuthority)
            return;

        PathCalculateAndMove();
    }

    private void PathCalculateAndMove()
    {
        if (IsDeath)
        {
            kcc.ResetVelocity();
            OnMoveVelocityChanged?.Invoke(0);
            
            return;
        }
        
        if (GetInput(out heroInput))
        {
            if (heroInput.HitPosition_RightClick != Vector3.zero)
            {
                lastPos = heroInput.HitPosition_RightClick;
                //IsCastingSkill = false;
            }
        }
        ButtonsPrevious = heroInput.Buttons;
        
        // 길찾기 경로 계산
        navMeshAgent.CalculatePath(lastPos, path);
        
        if (path.corners.Length <= 0)
        {   //정지 애니메이션 
            OnMoveVelocityChanged?.Invoke(0);
        }
        
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
                OnMoveVelocityChanged?.Invoke(0);
                return;
            }
            
            var speed = baseSpeed * (60f / Runner.TickRate);
            var direction = (nextWaypoint - kcc.Position).normalized;
            
           
            if (!IsCastingSkill)
            {
                kcc.Move(direction * speed);
            }
            else if(IsCastingSkill)  //스킬 사용했다면 이동 중지
            {
                //Debug.Log("ddddddddddddddddddddddddddddd");
                kcc.ResetVelocity();
                OnMoveVelocityChanged?.Invoke(0);
                return;
            }
            
            
            
            OnMoveVelocityChanged?.Invoke(1);
            
            float targetYaw = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float currentPitch = kcc.GetLookRotation(true, false).x;
            
            kcc.SetLookRotation(currentPitch, targetYaw);
            
        }
    }
}