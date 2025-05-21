using Fusion;
using UnityEngine;

public class PlayerAnimation : NetworkBehaviour
{
    
    [SerializeField] private Animator animator;
    private NewPlayerMovement movement;
    
     //private float MoveSpeed;
    private void Awake()
    {
       
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        movement = GetComponent<NewPlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
    }
    
    public override void Render()
    {
        animator.SetFloat("MoveSpeed", movement.navMeshAgent.velocity.magnitude);
    }
}
