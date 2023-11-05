using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState {Idle = 0 , Walk, Jump, Die }   // YS: �÷��̾� ���� 
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private KeyCode         jumpKey = KeyCode.Space;

    public PlayerState     playerState;

    [Header("���� ����Ʈ")]
    [SerializeField]
    private GameObject      effectDust;
    [SerializeField]
    private Transform       parent;
    [SerializeField]
    private float           delayTime;
    [SerializeField]
    private bool            isSpawning = false; // ���� ������ ����


    [Header("����")]
    [SerializeField]
    public float            lastMoveDir;

    private Movement2D      movement;
    private Animator        ani;
    private PoolManager     poolManager;
    

    private void Awake()
    {
        movement        = GetComponent<Movement2D>();
        ani             = GetComponent<Animator>();
        poolManager     = new PoolManager(effectDust);
    }

    private void OnApplicationQuit()
    {
        poolManager.DestroyObjcts();
    }

    private void Start()
    {
        ChangeState(PlayerState.Idle);
    }

    private void Update()
    {
        UpdateMove();
        UpdateJump();
        UpdateSight();
        ChangeAnimation();

        if(!isSpawning )
        {
            StartCoroutine("UpdateDustEffect");
        }
        
    }

    //======================================================================================
    // YS: �÷��̾� ������
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
        Vector2 mousPos = Input.mousePosition;
        Vector2 target  = Camera.main.ScreenToWorldPoint(mousPos);

        if (target.x < transform.position.x)
            transform.rotation = Quaternion.Euler(0, 180, 0);
        else
            transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    public IEnumerator UpdateDustEffect()
    {
        isSpawning = true;
        while(movement.rigidbody.velocity.x != 0 && movement.rigidbody.velocity.y == 0)
        {
            GameObject dustEffect = poolManager.ActivePoolItem();
            dustEffect.transform.position = parent.position;
            dustEffect.transform.SetParent(parent);
            dustEffect.GetComponent<PlayerDustEffect>().Setup(poolManager);
            yield return new WaitForSeconds(delayTime);
        }
        isSpawning = false;
    }

    //======================================================================================
    // YS: �÷��̾� ���� ����
    //======================================================================================

    public void ChangeState(PlayerState newState)
    {
        playerState = newState;
    }

    public void ChangeAnimation()
    {
        // �ȴ� ����
        if(movement.rigidbody.velocity.x != 0)
        {
            ChangeState(PlayerState.Walk);
            ani.SetFloat("MoveSpeed", movement.rigidbody.velocity.x);
        }
        // ���� ����
        if(movement.isJump == true)
        {
            ChangeState(PlayerState.Jump);
            ani.SetBool("IsJump", true);
        }
        // �״� ����
        if(Input.GetKeyDown(KeyCode.Q))
        {
            ChangeState(PlayerState.Die);
            ani.SetBool("IsDie", true);
        }
        // �⺻ ����
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
