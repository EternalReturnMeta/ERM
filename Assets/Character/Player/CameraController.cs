using System;
using Fusion;
using Unity.Cinemachine;
using UnityEngine;

public class CameraController : NetworkBehaviour
{
    public Camera _Camera;
    public CinemachineCamera _CinemachineCamera;
    public Transform CameraPoint;

    public float CameraMoveSpeed = 5f; // 카메라 이동 속도
    public float ScreenEdgeThreshold = 10f;

    private void Start()
    {
        _Camera = Camera.main;
    }

    public override void Spawned()
    {
        if(HasInputAuthority)
        {
             GameObject obj = GameObject.FindWithTag("CinemachineCamera");
             _CinemachineCamera = obj.GetComponent<CinemachineCamera>();
        }
    }

    private void Update()
    {
        if (!HasInputAuthority) return;
        CameraMoveToScreenEdge();

        if (Input.GetKey(KeyCode.Space))
        {
            _CinemachineCamera.transform.position = CameraPoint.transform.position;
            if (_CinemachineCamera.Target.TrackingTarget == null)
            {
                _CinemachineCamera.Target.TrackingTarget = CameraPoint.transform;
            }
        }
        if(Input.GetKeyUp(KeyCode.Space))
        {
            _CinemachineCamera.Target.TrackingTarget = null;
        }
    }

    private void CameraMoveToScreenEdge()
    {
        // 마우스 현재 위치를 가져옵니다.
        Vector3 mousePosition = Input.mousePosition;
        
        // 화면 너비와 높이 가져오기
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        // 카메라 이동 벡터 초기화
        Vector3 cameraMoveDirection = Vector3.zero;

        // 마우스가 화면 왼쪽 경계에 있는지 확인
        if (mousePosition.x <= ScreenEdgeThreshold)
        {
            cameraMoveDirection.x = -2; // 왼쪽으로 이동
        }
        // 마우스가 화면 오른쪽 경계에 있는지 확인
        else if (mousePosition.x >= screenWidth - ScreenEdgeThreshold)
        {
            cameraMoveDirection.x = 2; // 오른쪽으로 이동
        }

        // 마우스가 화면 아래 경계에 있는지 확인
        if (mousePosition.y <= ScreenEdgeThreshold)
        {
            cameraMoveDirection.z = -2; // 아래로 이동
        }
        // 마우스가 화면 위 경계에 있는지 확인
        else if (mousePosition.y >= screenHeight - ScreenEdgeThreshold)
        {
            cameraMoveDirection.z = 2; // 위로 이동
        }

        // 카메라 이동
        if (cameraMoveDirection != Vector3.zero)
        {
            Vector3 moveVector = cameraMoveDirection.normalized * (CameraMoveSpeed * Time.deltaTime);
            //_Camera.transform.Translate(moveVector, Space.World);
            _CinemachineCamera.transform.Translate(moveVector, Space.World);
        }
    }
}
