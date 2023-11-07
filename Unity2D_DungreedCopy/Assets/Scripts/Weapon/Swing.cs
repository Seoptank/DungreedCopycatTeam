using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swing : MonoBehaviour
{
    public Transform player;

    public Transform SwingPos;
    public GameObject SwingObj;

    void Awake()
    {
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSight();
        SwingSword();
    }

    void UpdateSight()
    {
        // ���콺 ��ġ�� �����ͼ� ���� ��ǥ�� ��ȯ
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // �÷��̾� ��ġ���� ���콺 ��ġ������ ���� ����
        Vector3 directionToMouse = mousePosition - player.position;

        // ������Ʈ�� ���콺 �������� z�� ȸ��
        float angle = Mathf.Atan2(directionToMouse.y, directionToMouse.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        // ���콺 x��ǥ�� �о� scale���� -1�� �ٲپ� �̹��� ����
        Vector2 scale = transform.localScale;
        if (directionToMouse.x < 0)
        {
            scale.y = -1;
        }
        else
        {
            scale.y = 1;
        }
        transform.localScale = scale;
    }

    void SwingSword()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            GameObject instantSwing = Instantiate(SwingObj, SwingPos.position, transform.rotation);
        }
    }
}