using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Movement2D : MonoBehaviour
{

    [Header("좌/우 이동, 점프 변수")]
    [SerializeField]
    private float               moveSpeed = 3.0f;
    [SerializeField]
    private float               jumpForce = 6.0f;
    [SerializeField]
    private float               lowGravity = 0.7f;  // 점프키를 오래 누르고 있을때 적용되는 낮은 중력
    [SerializeField]
    private float               highGravity = 1.2f; // 일반적으로 적용되는 점프 
    [HideInInspector]
    public bool                 isJump = false;     // Jump상태 채크
    [HideInInspector]
    public bool                 isWalk = false;     // Walk상태 채크

    [Header("더블 점프")]
    public bool                 haveDoubleJump;
    [SerializeField]
    private int                 haveDoubleJump_MaxJumpCount = 2;
    [SerializeField]
    private int                 normalState_MaxJumpCount = 1;
    [SerializeField]
    private int                 curJumpCount;

    [Header("대쉬")]
    public bool                 isDashing = false;
    public float                dashDis = 3.0f;
    [SerializeField]
    private float               dashSpeed = 10.0f;
    [SerializeField]
    private float               delayTime = 1.0f;


    
    [Header("땅 채크")]
    [SerializeField]
    private LayerMask           collisionLayer;
    [HideInInspector]
    public bool                 isGrounded;
    private Vector3             footPos;

    

    public bool isLongJump { set; get; } = false;

    [HideInInspector]
    public  Rigidbody2D         rigidbody;
    private BoxCollider2D       boxCollider2D;
    private PlayerController    playerController;

    private PlayerState         playerState;

    private void Awake()
    {
        rigidbody           = GetComponent<Rigidbody2D>();
        boxCollider2D       = GetComponent<BoxCollider2D>();
        playerController    = GetComponent<PlayerController>();
    }
    private void FixedUpdate()
    {
        Bounds bounds = boxCollider2D.bounds;

        footPos = new Vector3(bounds.center.x, bounds.min.y);

        isGrounded = Physics2D.OverlapCircle(footPos, 0.02f, collisionLayer);
        
        if(isGrounded == true && rigidbody.velocity.y <= 0)
        {
            isJump = false;

            if (haveDoubleJump == true)
            {
                curJumpCount = haveDoubleJump_MaxJumpCount;
            }
           else if(haveDoubleJump == false)
            {
                curJumpCount = normalState_MaxJumpCount;
            }
        }

        if(isLongJump && rigidbody.velocity.y > 0)
        {
            rigidbody.gravityScale = lowGravity;
        }
        else
        {
            rigidbody.gravityScale = highGravity;
        }
    }

    public void MoveTo(float x)
    {
        rigidbody.velocity = new Vector2(x * moveSpeed, rigidbody.velocity.y);
    }

    public bool JumpTo()
    {
        if( curJumpCount > 0 )
        {
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, jumpForce);
            curJumpCount--;
            isJump = true;
            isWalk = false;
            return true;
        }
        return false;
    }

    public IEnumerator DashTo(Vector3 des)
    {
        isDashing = true;

        float dis           = Vector3.Distance(transform.position, des);
        float step          = (dashSpeed/dis) * Time.fixedDeltaTime;
        float t             = 0f;

        Vector3 startingPos = transform.position;

        while (t <=  1.0f)
        {
            t += step;
            rigidbody.MovePosition(Vector3.Lerp(startingPos, des, t));
            yield return new WaitForFixedUpdate();
        }
        isDashing = false;
    }

    //=====================================================================
    // YS: 플레이어 Effect MemoryPool
    //=====================================================================
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(footPos, 0.02f);
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, dashDis);


    }
}
