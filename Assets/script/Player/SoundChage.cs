using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundChage : MonoBehaviour
{
    public SoundManager SoundManager;
    public string playerTag = "Player"; // �÷��̾ ��Ÿ���� �±�

    private bool playerInsideAnyCollider = false; // �ݶ��̴� ���� �÷��̾ �ִ��� ���θ� �����ϴ� ����

    void Update()
    {
        // �ݶ��̴� ���� �÷��̾ �ִ����� �� �����Ӹ��� üũ�մϴ�.
        CheckPlayerInsideCollider();
    }

    void CheckPlayerInsideCollider()
    {
        // �ڽ� ������Ʈ�� ��� �ݶ��̴��� �����ɴϴ�.
        Collider[] childColliders = GetComponentsInChildren<Collider>();

        // �÷��̾� ������Ʈ�� ã���ϴ�.
        GameObject playerObject = GameObject.FindGameObjectWithTag(playerTag);
        if (playerObject == null)
        {
            Debug.LogError("�÷��̾ ã�� �� �����ϴ�.");
            return;
        }

        // �÷��̾��� ��ġ�� �����ɴϴ�.
        Vector3 playerPosition = playerObject.transform.position;

        // �ݶ��̴� ���� �÷��̾ �ִ��� ���θ� Ȯ���մϴ�.
        bool isPlayerInsideAnyCollider = false;
        foreach (Collider collider in childColliders)
        {
            if (collider.bounds.Contains(playerPosition))
            {
                isPlayerInsideAnyCollider = true;
                break;
            }
        }

        // ���� �����ӿ��� �÷��̾ �ݶ��̴� ���� ��������, ���� �����ӿ����� ���� ��쿡�� ����մϴ�.
        if (isPlayerInsideAnyCollider && !playerInsideAnyCollider && !GameManager.Instance.MoveStageON)
        {
            if(!SoundManager.BGMSource.clip != SoundManager.audioList[0])
            {
                SoundManager.SoundChange2();
                Debug.Log("�̹� ���1");
            }
            else
            {
                Debug.Log("�̹� ������Դϴ�.");
            }
        }
        // ���� �����ӿ��� �÷��̾ �ݶ��̴� ���� �־�����, ���� �����ӿ����� ���� ��쿡�� ����մϴ�.
        else if (!isPlayerInsideAnyCollider && playerInsideAnyCollider && !GameManager.Instance.MoveStageON)
        {
            SoundManager.SoundChange4();
            Debug.Log("���� ����");
        }

        // �÷��̾ �ݶ��̴� ���� �ִ��� ���θ� �����մϴ�.
        playerInsideAnyCollider = isPlayerInsideAnyCollider;
    }
}
