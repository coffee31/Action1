using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundChage : MonoBehaviour
{
    public SoundManager SoundManager;
    public string playerTag = "Player"; // 플레이어를 나타내는 태그

    private bool playerInsideAnyCollider = false; // 콜라이더 내에 플레이어가 있는지 여부를 저장하는 변수

    void Update()
    {
        // 콜라이더 내에 플레이어가 있는지를 매 프레임마다 체크합니다.
        CheckPlayerInsideCollider();
    }

    void CheckPlayerInsideCollider()
    {
        // 자식 오브젝트의 모든 콜라이더를 가져옵니다.
        Collider[] childColliders = GetComponentsInChildren<Collider>();

        // 플레이어 오브젝트를 찾습니다.
        GameObject playerObject = GameObject.FindGameObjectWithTag(playerTag);
        if (playerObject == null)
        {
            Debug.LogError("플레이어를 찾을 수 없습니다.");
            return;
        }

        // 플레이어의 위치를 가져옵니다.
        Vector3 playerPosition = playerObject.transform.position;

        // 콜라이더 내에 플레이어가 있는지 여부를 확인합니다.
        bool isPlayerInsideAnyCollider = false;
        foreach (Collider collider in childColliders)
        {
            if (collider.bounds.Contains(playerPosition))
            {
                isPlayerInsideAnyCollider = true;
                break;
            }
        }

        // 이전 프레임에서 플레이어가 콜라이더 내에 없었지만, 현재 프레임에서는 있을 경우에만 출력합니다.
        if (isPlayerInsideAnyCollider && !playerInsideAnyCollider && !GameManager.Instance.MoveStageON)
        {
            if(!SoundManager.BGMSource.clip != SoundManager.audioList[0])
            {
                SoundManager.SoundChange2();
                Debug.Log("이미 재생1");
            }
            else
            {
                Debug.Log("이미 재생중입니다.");
            }
        }
        // 이전 프레임에서 플레이어가 콜라이더 내에 있었지만, 현재 프레임에서는 없을 경우에만 출력합니다.
        else if (!isPlayerInsideAnyCollider && playerInsideAnyCollider && !GameManager.Instance.MoveStageON)
        {
            SoundManager.SoundChange4();
            Debug.Log("사운드 변경");
        }

        // 플레이어가 콜라이더 내에 있는지 여부를 저장합니다.
        playerInsideAnyCollider = isPlayerInsideAnyCollider;
    }
}
