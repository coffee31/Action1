using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.TextCore.Text;

public class CameraSet : MonoBehaviour
{
    public float zoomSpeed = 5f;
    public float MinFOV = 10f;
    public float MaxFOV = 60f;

    public float rotationSpeed = 5.0f;

    [SerializeField] CinemachineVirtualCamera cinemachineCamera;

    void Update()
    {
        CamFov();

    }

    // Camera script

    void CamFov()
    {
        float scrollWheelInput = Input.GetAxis("Mouse ScrollWheel");
        float newFOV = cinemachineCamera.m_Lens.FieldOfView - scrollWheelInput * zoomSpeed;

        // FOV가 minFOV와 maxFOV 사이에 있도록 제한합니다.
        newFOV = Mathf.Clamp(newFOV, MinFOV, MaxFOV);

        // 카메라의 FOV를 업데이트합니다.
        cinemachineCamera.m_Lens.FieldOfView = newFOV;
    }


}
