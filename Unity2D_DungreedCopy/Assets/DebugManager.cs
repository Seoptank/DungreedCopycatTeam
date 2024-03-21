using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugManager : MonoBehaviour
{
    // �̱��� �ν��Ͻ�
    private static DebugManager instance;

    // �̱��� �ν��Ͻ��� ������ �� �ִ� ������Ƽ
    public static DebugManager Instance
    {
        get
        {
            // �ν��Ͻ��� ���� ��� ����
            if (instance == null)
            {
                GameObject debugManager = new GameObject("DebugManager");
                instance = debugManager.AddComponent<DebugManager>();
            }
            return instance;
        }
    }

    public int targetPosInQuadrant;

    public void DebugTargetQuadrant(Vector2 targetPos)
    {
        // ��ũ���� �߽� ��ǥ
        Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);

        // ���콺�� ��ũ�� ��ǥ�� �߽��� �������� �����¿� ��� ������ Ȯ���մϴ�.
        bool isTargetRight = targetPos.x > screenCenter.x;
        bool isTargetLeft = !isTargetRight;
        bool isTargetUp = targetPos.y > screenCenter.y;
        bool isTargetDown = !isTargetUp;

        // ���콺�� ��ġ�� ��� ���и鿡 �ִ��� ����� �α׷� ����մϴ�.
        if (isTargetRight && isTargetUp)
        {
            Debug.Log("Target�� ���ܿ� ��ġ�մϴ�.");
            targetPosInQuadrant = 1;
        }
        else if (isTargetLeft && isTargetUp)
        {
            Debug.Log("Target�� �»�ܿ� ��ġ�մϴ�.");
            targetPosInQuadrant = 2;
        }
        else if (isTargetLeft && isTargetDown)
        {
            Debug.Log("Target�� ���ϴܿ� ��ġ�մϴ�.");
            targetPosInQuadrant = 3;
        }
        else if (isTargetRight && isTargetDown)
        {
            Debug.Log("Target�� ���ϴܿ� ��ġ�մϴ�.");
            targetPosInQuadrant = 4;
        }
    }
}
