using System;
using UnityEngine;

public class PlayerCameraPoint : MonoBehaviour
{    
    [SerializeField] private Transform player;
    
    [SerializeField] private float FixedYPosition = 15;

    [SerializeField] private Vector3 DistanceOffset;
    void Start()
    {
        DistanceOffset = transform.position - player.position;
    }

    private void LateUpdate()
    {
        if (player != null)
        {
            // 플레이어의 X, Z 좌표를 기반으로 카메라 위치를 업데이트하되, Y는 고정
            Vector3 targetPosition = new Vector3(
                player.position.x + DistanceOffset.x,
                FixedYPosition,
                player.position.z + DistanceOffset.z
            );

            transform.position = targetPosition;
        }

    }

    void Update()
    {
        
    }
}
