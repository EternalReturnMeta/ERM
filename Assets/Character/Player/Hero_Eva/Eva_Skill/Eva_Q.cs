using Fusion;
using UnityEngine;

public class Eva_Q : NetworkBehaviour
{
    [Networked] private TickTimer life { get; set; }
    

    public override void Spawned()
    {
        life = TickTimer.CreateFromSeconds(Runner, 5.0f);
    }

    public override void FixedUpdateNetwork()
    {
        if(life.Expired(Runner))
            Runner.Despawn(Object);
        else
            transform.position += 15 * transform.forward * Runner.DeltaTime;
    }
   
}
