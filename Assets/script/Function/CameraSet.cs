using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSet : MonoBehaviour
{
    public float zoomSpeed = 5f;
    public float MinFOV = 10f;
    public float MaxFOV = 60f;

    [SerializeField] CinemachineVirtualCamera cinemachineCamera;

    void Update()
    {
        float scrollWheelInput = Input.GetAxis("Mouse ScrollWheel");
        float newFOV = cinemachineCamera.m_Lens.FieldOfView - scrollWheelInput * zoomSpeed;

        // FOV�� minFOV�� maxFOV ���̿� �ֵ��� �����մϴ�.
        newFOV = Mathf.Clamp(newFOV, MinFOV, MaxFOV);

        // ī�޶��� FOV�� ������Ʈ�մϴ�.
        cinemachineCamera.m_Lens.FieldOfView = newFOV;
    }
}
