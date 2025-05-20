using Fusion;
using Fusion.Addons.KCC;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
   [SerializeField] private KCC KCC;

   private void Awake()
   {
      KCC = GetComponent<KCC>();
      if (KCC == null)
      {
         Debug.LogError("KCC component is missing on this object!");
      }
      
      if (!Runner || !Runner.IsRunning)
      {
         Debug.LogError("NetworkRunner is not running or not set.");
      }

   }

   public override void Render()
   {
      
   }
   
   public override void FixedUpdateNetwork()
   {
      if (Runner.TryGetInputForPlayer(Object.InputAuthority, out PlayerInputData input) == true)
      {
         // Apply look rotation delta. This propagates to Transform component immediately.
         KCC.AddLookRotation(input.LookRotationDelta);

         // Set world space input direction. This value is processed later when KCC executes its FixedUpdateNetwork().
         // By default the value is processed by EnvironmentProcessor - which defines base character speed, handles acceleration/friction, gravity and many other features.
         Vector3 inputDirection = KCC.Data.TransformRotation * new Vector3(input.MoveDirection.x, 0.0f, input.MoveDirection.y);
         KCC.SetInputDirection(inputDirection);
         
         Debug.Log(inputDirection);
      }
   }
   
   
}
