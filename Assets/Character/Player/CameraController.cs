using Fusion;
using Unity.Cinemachine;
using UnityEngine;

public class CameraController : NetworkBehaviour
{
    public Camera _Camera;
    public CinemachineCamera _CinemachineCamera;
    public Transform CameraPoint;
   
    public override void Spawned()
    {
        Debug.Log(Object.Runner.LocalPlayer);
        
        if(HasInputAuthority)
        {
            GameObject obj = GameObject.FindWithTag("CinemachineCamera");
            _CinemachineCamera = obj.GetComponent<CinemachineCamera>();
            _CinemachineCamera.Target.TrackingTarget = CameraPoint.transform;
        }
    }
}
