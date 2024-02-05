using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagneticCoin : MonoBehaviour
{
    [Header("�ڼ� ȿ��")]
    private float       magnetDis;
    [SerializeField]
    private float       magnetStrngth;
    [SerializeField]
    private int         magnetDirection = 1; // �η��� 1, ô���� -1

    [Header("��� ����")]
    public int         goldValue;

    [SerializeField]
    private GameObject          textGoldPrefab;
    private PoolManager         TextGoldpoolManager;
    
    private Transform           playerTransform;
    private PoolManager         poolManager;
    private GoldController      goldController;
    private PlayerStats         playerStats;
    private Rigidbody2D         rigidbody2D;


    public void Setup(PoolManager newPool)
    {
        poolManager = newPool;

        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        goldController = FindObjectOfType<GoldController>();
        playerStats = FindObjectOfType<PlayerStats>();

        rigidbody2D = GetComponent<Rigidbody2D>();

        magnetDis = goldController.magnetDis;

    }

    private void Awake()
    {
        TextGoldpoolManager = new PoolManager(textGoldPrefab);
    }

    private void Update()
    {
        CheckDisToPlayer();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            // ��Ȱ��ȭ
            poolManager.DeactivePoolItem(gameObject);

            // �÷��̾� �� ��忡 �߰��ϴ� ��ũ��Ʈ
            playerStats.TakeGold(goldValue);

            // �ؽ�Ʈ Ȱ��ȭ
            ActivateGoldText();
        }
    }

    private void ActivateGoldText()
    {
        GameObject goldText = TextGoldpoolManager.ActivePoolItem();
        goldText.transform.position = transform.position;
        goldText.transform.rotation = transform.rotation;
        goldText.GetComponent<TextGoldController>().Setup(TextGoldpoolManager);
    }
    private void CheckDisToPlayer()
    {
        Vector2 dirToTarget= playerTransform.position - transform.position;
        float dis = Vector2.Distance(playerTransform.position, transform.position);
        float magnetDisStr = (magnetDis / dis) * magnetStrngth;
        transform.Translate(magnetDisStr * (dirToTarget * magnetDirection) * Time.deltaTime);
    }
}
