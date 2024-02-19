using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// == NPC ID���==
// ũ��(���� ����) => 10

public class NPCManager : MonoBehaviour
{
    [Header("NPC UI")]
    [SerializeField]
    private GameObject  keyObj;
    
    [Header("NPC Data")]
    public int          ID;             // Talk�� Dictionary�� ���� key��
    public string[]     talkSentences;
    public string       npcName;


    private KeyCode     activateChatKey = KeyCode.F;    // ��ȭâ�� Ȱ��ȭ ��ų Key
    private bool        isActivateKey;                  // KeyUI�� Ȱ��/��Ȱ��ȭ 
    private bool        inputKey;                       // key�� ���ȴ��� ����
    private void Update()
    {
        keyObj.SetActive(isActivateKey);    

        // Key�� ���� �� �ִ� ���� ���� �����鼭 Key�� ������
        if(Input.GetKeyDown(activateChatKey) && isActivateKey)
        {
            // Talk�� Data�� TalkManager�� Dictionary�� ���� 
            TalkManager.Instance.AddTalkData(ID, talkSentences);
            // Key�� ������ KeyUI�� Ȱ��ȭ���� �ʵ���
            isActivateKey = false;
            // KeyȰ��ȭ ������ Ȱ��ȭ ���� �ʵ���
            inputKey      = true;

            UIManager.instance.OnTalkPanel();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "Player"&& !inputKey)
        {
            isActivateKey = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player"&& !inputKey)
        {
            isActivateKey = false;
        }
    }
}
