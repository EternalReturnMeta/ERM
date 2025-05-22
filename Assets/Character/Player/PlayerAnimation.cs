using Fusion;
using UnityEngine;

public class PlayerAnimation : NetworkBehaviour
{
    
    [SerializeField] private Animator animator;
    private PlayerMovement movement;
    
    void Start()
    {
        movement = GetComponent<PlayerMovement>();
    }
    
    
    public override void Render()
    {
        if (animator)
        {
            if (movement)
            {
                animator.SetFloat("MoveSpeed", movement.navMeshAgent.velocity.magnitude);
            }
        }
    }
}
