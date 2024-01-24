using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Movement2D : MonoBehaviour
{
    [Header("MoveX,Jump")]
    [SerializeField]
    private float           moveSpeed = 3.0f;
    [SerializeField]        
    private float           jumpForce = 8.0f;
    [SerializeField]        
    private float           downJumpForce;
    [SerializeField]        
    private float           lowGravity = 1.0f;      // 점프키를 오래 누르고 있을때 적용되는 낮은 중력
    [SerializeField]        
    private float           highGravity = 1.5f;     // 일반적으로 적용되는 점프 
    public bool             isJump = false;         // Jump상태 채크
    public bool             isdownJump = false;     // Jump상태 채크
    public bool             isWalk = false;         // Walk상태 채크
    [SerializeField]
    private int             playerLayer, platformLayer;
    [SerializeField]
    private float           downJumpTime;


    [Header("DoubleJump")]
    public bool             haveDoubleJump;
    [SerializeField]
    private int             haveDoubleJump_MaxJumpCount = 2;
    [SerializeField]
    private int             normalState_MaxJumpCount = 1;
    [SerializeField]
    private int             curJumpCount;

    [Header("Checking Slope")]
    [SerializeField]
    private float           dis;
    [SerializeField]
    private float           angle;
    [SerializeField]
    private float           maxAngle;   // YS: 최대 각도를 설정해 이 각도 이상으로는 못올라가게 설정할 수 있음
    [SerializeField]
    private bool            isSlope = false;
    [SerializeField]
    private Vector2         prep;
    
    [Header("Checking Ground")]
    [SerializeField]
    private LayerMask       collisionLayer;
    public bool             isGrounded;
    [SerializeField]
    private Transform       footPos;
    [SerializeField]
    private float           checkRadius;
    
    [Header("Dash")]
    public bool             isDashing = false;
    public float            dashDis = 5.0f;
    [SerializeField]
    private float           dashSpeed = 20.0f;
    public float            ghostDelay;
    [SerializeField]
    private float           ghostDelaySeconds = 1.0f;
    [SerializeField]
    private GameObject      dashPrefab;
    [SerializeField]
    public Vector3          dashDir;
    private PoolManager     dashPoolManager;
    
    [Header("Dash Count")]
    public int              maxDashCount = 3;
    public int              curDashCount;
    public float            dashCountChargeDelayTime = 5.0f;

    [Header("DustEffect")]
    private PoolManager     dustPoolManager;
    [SerializeField]
    private GameObject      dustPrefab;
    [SerializeField]
    private bool            isSpawningDust = false;
    
    [Header("JumpEffect")]
    private PoolManager     jumpDustPoolManager;
    [SerializeField]
    private GameObject      jumpDustPrefab;

    [Header("DoubleJumpEffect")]
    private PoolManager     doubleJumpDustPoolManager;
    [SerializeField]
    private GameObject      doubleJumpDustPrefab;


    public bool             isLongJump { set; get; } = false;

    [HideInInspector]
    public Rigidbody2D              rigidbody;
    private BoxCollider2D           boxCollider2D;
    private PlayerStats             playerStats;


    private void Awake()
    {
        rigidbody           = GetComponent<Rigidbody2D>();
        boxCollider2D       = GetComponent<BoxCollider2D>();
        playerStats         = GetComponent<PlayerStats>();
        
        dashPoolManager             = new PoolManager(dashPrefab);
        dustPoolManager             = new PoolManager(dustPrefab);
        jumpDustPoolManager         = new PoolManager(jumpDustPrefab);
        doubleJumpDustPoolManager   = new PoolManager(doubleJumpDustPrefab);
    }
    private void Start()
    {
        ghostDelaySeconds   = ghostDelay;

        // YS: Dash변수 초기화
        curDashCount        = maxDashCount;

        // YS: 레이어 초기화
        playerLayer     = LayerMask.NameToLayer("Player");
        platformLayer   = LayerMask.NameToLayer("PassingPlatform");
    }
    
    private void OnApplicationQuit()
    {
        dashPoolManager.DestroyObjcts();
        dustPoolManager.DestroyObjcts();
        jumpDustPoolManager.DestroyObjcts();
        doubleJumpDustPoolManager.DestroyObjcts();
    }
    private void FixedUpdate()
    {
        RaycastHit2D hit        = Physics2D.Raycast(this.transform.position, Vector2.down, dis, collisionLayer);
        CheckSlope(hit);

        GroundCheckAndJumpType();

        if (isDashing)
        {
            ActiveDashEffect();
        }
        

        if(isJump && rigidbody.velocity.y <= 0)
        {
            GameObject.FindWithTag("PassingPlatform").GetComponent<Passing>().OffPassing(playerLayer,platformLayer);
        }

        Physics2D.IgnoreLayerCollision(playerLayer, LayerMask.NameToLayer("Platform"), false);
    }
    public void MoveTo(float x)
    {
        if (isSlope && isGrounded && !isJump && angle < maxAngle)
            rigidbody.velocity = prep * moveSpeed * x * -1f;
        else if (!isSlope && isGrounded && !isJump)
            rigidbody.velocity = new Vector2(x * moveSpeed, 0);
        else if (!isGrounded)
            rigidbody.velocity = new Vector2(x * moveSpeed, rigidbody.velocity.y);


        // DustEffect Active
        if (!isSpawningDust)
        {
            StartCoroutine("ActiveDustEffect");
        }
    }

    public bool JumpTo()
    {
        if (curJumpCount > 0)
        {
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, jumpForce);
            curJumpCount--;
            isJump = true;
            isWalk = false;

            if(haveDoubleJump == true && curJumpCount < 1 && Input.GetKeyDown(KeyCode.Space))
            {
                ActiveDoubleJumpDustEffect();
            }

            if(isGrounded)
            {
                ActiveJumpDustEffect();
            }

            // YS: 점프중 Platform 무시
            if (rigidbody.velocity.y > 0)
            {
                GameObject.FindWithTag("PassingPlatform").GetComponent<Passing>().OnPassing(playerLayer, platformLayer);
            }
            
            return true;
            
        }
        return false;
    }

    public void DownJumpTo()
    {
        StartCoroutine(GameObject.FindWithTag("PassingPlatform").GetComponent<Passing>().PassingRoutain(playerLayer, platformLayer, downJumpTime));
        rigidbody.velocity = Vector2.down * downJumpForce;
    }
    private void CheckSlope(RaycastHit2D hit)
    {
        if(hit)
        {
            // YS: Vector2.Perpendicular(Vector2 A)는 값에서 "반시계 방향"으로 90도 회전한
            //     벡터값을 반환한다.

            // YS: hit.normal은 충돌한 지점에서 면에 수직인 법선 벡터임.
            prep = Vector2.Perpendicular(hit.normal).normalized;
            angle = Vector2.Angle(hit.normal, Vector2.up);

            // YS: 각도가 0이 아님으로 경사일때
            if(angle != 0) isSlope = true;
            else           isSlope = false;

            Debug.DrawLine(hit.point, hit.point + hit.normal, Color.green);
            Debug.DrawLine(hit.point, hit.point + prep, Color.red);
        }
    }
    private void GroundCheckAndJumpType()
    {
        isGrounded = Physics2D.OverlapCircle(footPos.position, checkRadius, collisionLayer);
        
        if (isGrounded == true && rigidbody.velocity.y <= 0)
        {
            isJump = false;
            isdownJump = false;

            if (haveDoubleJump == true)
            {
                curJumpCount = haveDoubleJump_MaxJumpCount;
            }
            else if (haveDoubleJump == false)
            {
                curJumpCount = normalState_MaxJumpCount;
            }
        }

        if (isLongJump && rigidbody.velocity.y > 0)
        {
            rigidbody.gravityScale = lowGravity;
        }
        else
        {
            rigidbody.gravityScale = highGravity;
        }
    }
    public void PlayDash()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        dashDir = mousePos - transform.position;
        Vector3 moveTarget = transform.position + Vector3.ClampMagnitude(dashDir, dashDis);
        if(playerStats.DC > 0)
        {
            StartCoroutine(DashTo(moveTarget));
            playerStats.UseDC();
        }
    }
    private IEnumerator DashTo(Vector3 moveTarget)
    {
        isDashing = true;
        curDashCount--;

        float dis = Vector3.Distance(transform.position, moveTarget);
        float step = (dashSpeed / dis) * Time.fixedDeltaTime;
        float t = 0f;

        Vector3 startingPos = transform.position;

        GameObject.FindWithTag("PassingPlatform").GetComponent<Passing>().OnPassing(playerLayer, platformLayer);

        while (t <= 1.0f)
        {
            t += step;
            rigidbody.MovePosition(Vector3.Lerp(startingPos, moveTarget, t));
            yield return new WaitForFixedUpdate();
        }
        playerStats.timer = 0;
        isDashing = false;
        GameObject.FindWithTag("PassingPlatform").GetComponent<Passing>().OffPassing(playerLayer, platformLayer);

    }
    //=====================================================================
    // YS: Player Effect Active
    //=====================================================================
    private IEnumerator ActiveDustEffect()
    {
        isSpawningDust = true;
        while (rigidbody.velocity.x != 0 && !isJump)
        {
            GameObject dustEffect = dustPoolManager.ActivePoolItem();
            dustEffect.transform.position = transform.position + new Vector3(0,-0.25f,-1f);
            dustEffect.transform.rotation = transform.rotation;
            dustEffect.GetComponent<EffectPool>().Setup(dustPoolManager);
            yield return new WaitForSeconds(0.3f);
        }
        isSpawningDust = false;
    }
    public void ActiveJumpDustEffect()
    {
        GameObject jumpDustEffect = jumpDustPoolManager.ActivePoolItem();
        jumpDustEffect.transform.position = transform.position + new Vector3(0, -0.25f, 0);
        jumpDustEffect.transform.rotation = transform.rotation;
        jumpDustEffect.GetComponent<EffectPool>().Setup(jumpDustPoolManager);
    }
    public void ActiveDoubleJumpDustEffect()
    {
        GameObject doubleJumpDustEffect = doubleJumpDustPoolManager.ActivePoolItem();
        doubleJumpDustEffect.transform.position = transform.position + new Vector3(0, -0.25f, 0);
        doubleJumpDustEffect.transform.rotation = transform.rotation;
        doubleJumpDustEffect.GetComponent<EffectPool>().Setup(doubleJumpDustPoolManager);
    }
    private void ActiveDashEffect()
    {
        if (ghostDelaySeconds > 0)
        {
            ghostDelaySeconds -= Time.deltaTime;
        }
        else
        {
            GameObject ghostEffect = dashPoolManager.ActivePoolItem();
            ghostEffect.transform.position = transform.position;
            ghostEffect.transform.rotation = transform.rotation;
            ghostEffect.GetComponent<EffectPool>().Setup(dashPoolManager);
            Sprite curSprite = GetComponent<SpriteRenderer>().sprite;
            ghostEffect.GetComponent<SpriteRenderer>().sprite = curSprite;
            ghostDelaySeconds = ghostDelay;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(footPos.position, checkRadius);
    }
}
