using System;
using System.Collections;
using Fusion;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : NetworkBehaviour
{
    private Camera mainCamera;
    [SerializeField] private LayerMask groundLayer; // 바닥 레이어
    [HideInInspector] public NavMeshAgent navMeshAgent;
    
    [Networked] public NetworkButtons ButtonsPrevious { get; set; }

    private HeroInput heroInput;
    private int moveCheck;
    public event Action<int> OnMove;
    
    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        moveCheck = 0;
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
        //Debug.Log("1111111111111");
        if (!HasStateAuthority) return;
       
        if (GetInput(out heroInput))
        {
            if(heroInput.Buttons.WasPressed(ButtonsPrevious, InputButton.RightClick))
            {
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
                    if (moveCheck == 1)
                    {
                        OnMove?.Invoke(0);
                        moveCheck = 0;
                    }
                }
            }
        }
    }
    
    private void ToggleProcess(bool isOn)
    {
        if (isOn)
        {
            StartCoroutine(SetPositionProcess());
        }
        else
        {
            StopAllCoroutines();
            moveCheck = 1;
        }
    }

    private IEnumerator SetPositionProcess()
    {
        while (true)
        {
            OnMove?.Invoke(1);
            
            navMeshAgent.isStopped = false;
            navMeshAgent.SetDestination(heroInput.HitPosition);
            
            yield return new WaitForSeconds(0.15f);
        }
    }
    
}
