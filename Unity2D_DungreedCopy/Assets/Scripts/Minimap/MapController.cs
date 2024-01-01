using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    [Header("���� ��")]
    [SerializeField]
    private GameObject[]        dungeonMaps;
    public List<string>         dungeonNames;

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
        UpdateDungeonMapUI();
    }

    private void DontActivateDungeonMap()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            MapOn       = !MapOn;
            MiniMapOn   = !MiniMapOn;

            MapUI.SetActive(MapOn);
            MiniMapUI.SetActive(MiniMapOn);
        }
    }

    private void UpdateDungeonMapUI()
    {
        for (int i = 0; i < dungeonMaps.Length; ++i)
        {
            if(dungeonNames.Contains(dungeonMaps[i].name))
            {
                dungeonMaps[i].SetActive(true);
            }
            else
            {
                dungeonMaps[i].SetActive(false);
            }

        }
    }
}
