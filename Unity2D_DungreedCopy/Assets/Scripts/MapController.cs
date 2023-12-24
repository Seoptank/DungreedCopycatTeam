using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public GameObject MapUI;
    public GameObject MiniMapUI;

    private bool MapOn = false;
    private bool MiniMapOn = true;

    // YS: �÷��̾� ��Ʈ�ѷ� ��ũ��Ʈ���� curScenename�� �޾ƿ� Village������ �������� ������ �ʰ� �ϱ� ����
    private PlayerController player;

    private void Awake()
    {
        player = FindObjectOfType<PlayerController>();
    }
    void Update()
    {
        DontActivateDungeonMap();
    }

    private void DontActivateDungeonMap()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if(player.curSceneName == "Village")
            {
                MapOn       = false;
                MiniMapOn   = true;

            }
            else
            {
                MapOn       = !MapOn;
                MiniMapOn   = !MiniMapOn;
            }

            MapUI.SetActive(MapOn);
            MiniMapUI.SetActive(MiniMapOn);
        }
    }
}
