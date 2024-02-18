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
    [SerializeField]
    private int         ID;             // Talk�� Dictionary�� ���� key��
    [SerializeField]
    private string[]    talkSentences;
    [SerializeField]
    private string      name;


    private KeyCode     activateChatKey = KeyCode.F;    // ��ȭâ�� Ȱ��ȭ ��ų Key
    private bool        isActivateKey;                  // KeyUI�� Ȱ��/��Ȱ��ȭ 
    private bool        inputKey;                       // key�� ���ȴ��� ����
    
    //private TalkManager talkManager;

    private void Awake()
    {
        //talkManager = FindObjectOfType<TalkManager>();
        
        //talkManager.AddTalkData(ID, talkSentences);
        
    }
    private void Update()
    {
        keyObj.SetActive(isActivateKey);    

        // Key�� ���� �� �ִ� ���� ���� �����鼭 Key�� ������
        if(Input.GetKeyDown(activateChatKey) && isActivateKey)
        {
            // Talk�� Data�� TalkManager�� Dictionary�� ���� 
            TalkManager.Instance.AddTalkData(ID, name, talkSentences);
            // Key�� ������ KeyUI�� Ȱ��ȭ���� �ʵ���
            isActivateKey = false;
            // KeyȰ��ȭ ������ Ȱ��ȭ ���� �ʵ���
            inputKey      = true;
            
            // UIManagerȣ���� ���ڿ� ������ ���
            UIManager.instance.OnTalk(name,this.gameObject);
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
