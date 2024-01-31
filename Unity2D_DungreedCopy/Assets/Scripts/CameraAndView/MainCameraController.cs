using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraController : MonoBehaviour
{
    static public MainCameraController instance;

    private float           shakeTime;
    private float           shakeIntensity;

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

    private PlayerController playerController;

    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;

            playerController = FindObjectOfType<PlayerController>();
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
        if(playerController.playerMeetsBoss == false && playerController.isBossDie == false)
        {
            ChasePlayer();
        }

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

    public IEnumerator ChangeView(Transform changePos, float camMoveTime)
    {
        float elapsedTime = 0f;
        Vector3 targetPos = new Vector3(changePos.position.x, changePos.position.y, changePos.position.z - 10);
       
        while(elapsedTime< camMoveTime)
        {
            transform.position = Vector3.Lerp(transform.position, targetPos, elapsedTime / camMoveTime);
            elapsedTime += Time.deltaTime;

            yield return null;
        }

    }
    public void OnShakeCam(float shakeTime = 1.0f, float shakeIntensity = 0.1f)
    {
        this.shakeTime = shakeTime;
        this.shakeIntensity = shakeIntensity;

        StopCoroutine("ShakePos");
        StartCoroutine("ShakePos");
    }

    private IEnumerator ShakePos()
    {
        Vector3 StartPos = transform.position;

        while (shakeTime > 0.0f)
        {
            transform.position = StartPos + Random.insideUnitSphere * shakeIntensity;

            shakeTime -= Time.deltaTime;

            yield return null;
        }

        transform.position = StartPos;
    }
    public void ChasePlayer()
    {
        Vector3 targetPos = new Vector3(player.position.x, player.position.y, this.transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPos, smooting);
    }
}
