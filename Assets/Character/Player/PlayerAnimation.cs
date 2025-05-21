using Fusion;
using UnityEngine;

public class PlayerAnimation : NetworkBehaviour
{
    
    [SerializeField] private Animator animator;
    private NewPlayerMovement movement;
    
    private void Awake()
    {
       
    }
    void Start()
    {
        movement = GetComponent<NewPlayerMovement>();
    }

    void Update()
    {
    }
    
    public override void Render()
    {
        animator.SetFloat("MoveSpeed", movement.navMeshAgent.velocity.magnitude);
    }
}
