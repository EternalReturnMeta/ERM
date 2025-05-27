using System;
using System.Collections;
using Fusion;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : NetworkBehaviour
{
    private Camera mainCamera; // 카메라 참조
    [SerializeField] private LayerMask groundLayer; // 바닥 레이어
    [HideInInspector] public NavMeshAgent navMeshAgent;
    
    [Networked] public NetworkButtons ButtonsPrevious { get; set; }

    private HeroInput heroInput;
    
    private PlayerAnimation _PlayerAnimation;
    public event Action<int> OnMove;
    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        _PlayerAnimation = GetComponent<PlayerAnimation>();
    }
    
    private void Start()
    {
        mainCamera = Camera.main;
    }

    public override void Spawned()
    {   //클라이언트 agent 비활성화
        if (!HasStateAuthority)
        {
            var agent = GetComponentInChildren<NavMeshAgent>();
            agent.enabled = false;
        }
    }

    public override void FixedUpdateNetwork() 
    {
        if (!HasStateAuthority) return;
       
        if (GetInput(out heroInput))
        {
            if(heroInput.Buttons.WasPressed(ButtonsPrevious, InputButton.RightClick))
            {
                Debug.Log($"RightClick", gameObject);
                ToggleProcess(true);
            }
            if (heroInput.Buttons.WasReleased(ButtonsPrevious, InputButton.RightClick))
            {
                ToggleProcess(false);
            }
        }
        
        ButtonsPrevious = heroInput.Buttons;
    }

    public override void Render()
    {
        if (HasStateAuthority)
        {
            if (navMeshAgent)
            {
                if (navMeshAgent.remainingDistance < 0.1f)
                {
                    navMeshAgent.isStopped = true;
                    OnMove?.Invoke(0);
                }
            }
        }
    }
    
    private void ToggleProcess(bool isOn)
    {
        if (isOn)
        {
            StartCoroutine(SetPositionProcess());
            OnMove?.Invoke(1);
        }
        else
        {
            StopAllCoroutines();
        }
    }

    private IEnumerator SetPositionProcess()
    {
        while (true)
        {
            navMeshAgent.SetDestination(heroInput.HitPosition);
            navMeshAgent.isStopped = false;
            
            yield return new WaitForSeconds(0.15f);
        }
    }
    
}
