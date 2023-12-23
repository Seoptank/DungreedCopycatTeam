using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraController : MonoBehaviour
{
    static public MainCameraController instance;

    [SerializeField]
    private Transform       player;
    [SerializeField]
    private float           smooting = 0.2f;

    public BoxCollider2D    bound;

    // YS: �ڽ� �ݶ��̴� ������ �ּ�/ �ִ� x,y,z���� ���� ����
    private Vector3         minBound;
    private Vector3         maxBound;

    // YS: ī����� �ݳʺ�, �ݳ��� ���� ���� ����
    private float           halfWidth;
    private float           halfHeight;

    // YS: ī�޶��� �ݳ��� ���� �Ӽ��� �̿��ϱ� ���� ����
    private Camera          halfHeightCam;

    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    private void Start()
    {
        halfHeightCam = GetComponent<Camera>();
        
        minBound = bound.bounds.min;
        maxBound = bound.bounds.max;

        // YS: �ݳʺ� ���ϴ� ���� = �ݳ��� * Screen.width / Screen.height(Screen.���� �ػ󵵸� ��Ÿ��)
        halfHeight = halfHeightCam.orthographicSize;
        halfWidth = halfHeight * Screen.width / Screen.height;
    }
    private void FixedUpdate()
    {
        Vector3 targetPos = new Vector3(player.position.x, player.position.y, this.transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPos, smooting);

        float clampedX = Mathf.Clamp(this.transform.position.x, minBound.x + halfWidth, maxBound.x - halfWidth);
        float clampedY = Mathf.Clamp(this.transform.position.y, minBound.y + halfHeight, maxBound.y - halfHeight);

        this.transform.position = new Vector3(clampedX, clampedY, transform.position.z);
    }

    public void SetBound(BoxCollider2D newBound)
    {
        bound = newBound;
        minBound = bound.bounds.min;
        maxBound = bound.bounds.max;
    }
}
