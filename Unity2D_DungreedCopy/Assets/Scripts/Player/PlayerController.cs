using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState {Idle = 0 , Walk, Jump, Die }   // YS: 플레이어 상태 
public class PlayerController : MonoBehaviour
{
    [Header("방향")]
    [SerializeField]
    public float            lastMoveDir;
    [SerializeField]
    public Vector3          mousePos;

    [SerializeField]
    private KeyCode         jumpKey = KeyCode.Space;
    [SerializeField]
    private KeyCode         dashKey = KeyCode.Mouse1;

    public PlayerState     playerState;

    private Movement2D      movement;
    private Animator        ani;

    private void Awake()
    {
        movement        = GetComponent<Movement2D>();
        ani             = GetComponent<Animator>();
    }

    private void Start()
    {
        ChangeState(PlayerState.Idle);
    }

    private void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        UpdateMove();
        UpdateJump();
        UpdateSight();
        ChangeAnimation();
        UpdateDash();

        if (movement.isDashing) return;

    }

    //======================================================================================
    // YS: 플레이어 움직임
    //======================================================================================

    public void UpdateMove()
    {
        float x = Input.GetAxis("Horizontal");

        if (x != 0)
        {
            lastMoveDir = Mathf.Sign(x);
        }

        movement.MoveTo(x);
        movement.isWalk = true;
    }
    
    public void UpdateJump()
    {
        if(Input.GetKeyDown(jumpKey))
        {
            bool isJump = movement.JumpTo();
        }
        else if (Input.GetKey(jumpKey))
        {
            movement.isLongJump = true;
        }
        else if (Input.GetKeyUp(jumpKey))
        {
            movement.isLongJump = false;
        }
    }
    public void UpdateSight()
    {
        if (mousePos.x < transform.position.x)
            transform.rotation = Quaternion.Euler(0, 180, 0);
        else
            transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    public void UpdateDash()
    {
        if(Input.GetKeyDown(dashKey) && movement.isDashing == false)
        {
            mousePos.z = 0;
            Vector3 dir = mousePos - transform.position;
            Vector3 moveTarget = transform.position + Vector3.ClampMagnitude(dir, movement.dashDis);

            StartCoroutine(movement.DashTo(moveTarget));
        }
    }

    //======================================================================================
    // YS: 플레이어 상태 변경
    //======================================================================================

    public void ChangeState(PlayerState newState)
    {
        playerState = newState;
    }

    public void ChangeAnimation()
    {
        // 걷는 상태
        if(movement.rigidbody.velocity.x != 0)
        {
            ChangeState(PlayerState.Walk);
            ani.SetFloat("MoveSpeed", movement.rigidbody.velocity.x);
        }
        // 점프 상태
        if(movement.isJump == true)
        {
            ChangeState(PlayerState.Jump);
            ani.SetBool("IsJump", true);
        }
        // 죽는 상태
        if(Input.GetKeyDown(KeyCode.Q))
        {
            ChangeState(PlayerState.Die);
            ani.SetBool("IsDie", true);
        }
        // 기본 상태
        if(movement.isGrounded == true && movement.rigidbody.velocity.x ==0)
        {
            ChangeState(PlayerState.Idle);
            ani.SetFloat("MoveSpeed", movement.rigidbody.velocity.x);
        }
        if(movement.isGrounded == true)
        {
            ani.SetBool("IsJump", false);
        }
    }
}
