using Definition;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class BaseMove : MonoBehaviour
{
    [Header("이속 수치")]
    [Tooltip("이속")]
    [SerializeField] protected FStat moveSpeed = new FStat(5f);
    public FStat MoveSpeed { get { return moveSpeed; } }
    [Tooltip("최고 속도 도달 시간")]
    [SerializeField] protected float accelerateTime = 0.1f;

    [Tooltip("점프")]
    [SerializeField] protected FStat jumpForce = new FStat(3f);
    public FStat JumpForce { get { return jumpForce; } }

    [Header("중력들")]
    [Tooltip("기본 중력")]
    [SerializeField] protected float baseGravity = 10f;
    public float BaseGravity { get { return baseGravity; } set { baseGravity = value; } }

    [Tooltip("점프 상승시 중력")]
    [SerializeField] protected float jumpGravity = 5f;
    public float JumpGravity { get { return jumpGravity; } set { jumpGravity = value; } }



    [Header("기타 등등")]

    [Tooltip("이동 가능 레이어")]
    [SerializeField] protected LayerMask moveableLayer;
    public LayerMask MoveableLayer { get { return moveableLayer; } set { moveableLayer = value; } }

    [Tooltip("발 판정 크기")]
    [SerializeField] protected Vector2 feetSize = new Vector2(1f, 0.1f);
    // [SerializeField] protected BoxCollider2D groundCollider = null;

    virtual protected void Awake()
    {
        if (rigid == null)
        {
            TryGetComponent<Rigidbody2D>(out rigid);
        }

        if (col == null)
        {
            TryGetComponent<Collider2D>(out col);
        }

        if (col != null)
        {
            // groundCollider = this.gameObject.AddComponent<BoxCollider2D>();

            // groundCollider.size = groundColSize;

            Vector2 groundColPos = new Vector2(col.bounds.center.x, col.bounds.min.y);

            feetPosOffset = groundColPos - (Vector2)this.transform.position;

            // groundCollider.offset = groundColPos;
        }
    }

    virtual protected void Update()
    {
        CheckGround();
    }

    virtual protected void FixedUpdate()
    {
        JumpHeightUpdate();
        JumpAdditive();
    }

    /// <summary>
    /// 이동 담당. 현재로선 target받고 target에 이속 곱해주지만, 바뀔 수 있다.
    /// </summary>
    /// <param name="target"> 이동하고자 하는 방향.</param>
    virtual public void MoveTo(Vector3 target)
    {
        //curTargetPos = target;

        rigid.linearVelocity = target * moveSpeed.FinalStat();
    }

    private float curVelocity;
    virtual public void MoveToX(float x)
    {
        if (x != 0) x = Mathf.Sign(x);
        else
        {
            float newSpeed = Mathf.SmoothDamp(rigid.linearVelocityX, 0, ref curVelocity, accelerateTime);
            rigid.linearVelocityX = newSpeed;
            return;
        }

        if (x == Mathf.Sign(rigid.linearVelocityX))
        {
            if (Mathf.Abs(rigid.linearVelocityX) < x * x * moveSpeed.FinalStat())
            {
                float newSpeed = Mathf.SmoothDamp(rigid.linearVelocityX, x * moveSpeed.FinalStat(), ref curVelocity, accelerateTime);
                rigid.linearVelocityX = newSpeed;
            }
        }
        else
        {
            float newSpeed = Mathf.SmoothDamp(rigid.linearVelocityX, x * moveSpeed.FinalStat(), ref curVelocity, accelerateTime);
            rigid.linearVelocityX = newSpeed;
        }
    }

    [Header("점프")]
    [Tooltip("점프 선입력 시간")]
    [SerializeField] private float jumpBufferTime = 0;
    private float jumpBufferTimer = -1;

    [Tooltip("점프 유예 시간")]
    [SerializeField] private float coyoteTime = 0;
    private float coyoteTimer = -1;

    virtual public void Jump()
    {
        jumpBufferTimer = jumpBufferTime;
    }

    virtual protected void JumpHeightUpdate()
    {
        if (isLongJump && rigid.linearVelocityY >= 0)
        {
            rigid.gravityScale = jumpGravity;
        }
        else
        {
            rigid.gravityScale = baseGravity;
        }
    }

    virtual protected void JumpAdditive()
    {
        if (isGrounded) coyoteTimer = coyoteTime;
        else coyoteTimer -= Time.deltaTime;

        if (jumpBufferTimer > 0) jumpBufferTimer -= Time.deltaTime;

        if (jumpBufferTimer > 0 && coyoteTimer > 0)
        {
            rigid.linearVelocityY = jumpForce.FinalStat();
            jumpBufferTimer = 0;
            coyoteTimer = 0;
        }
    }

    /// <summary>
    /// 땅 체크 함수. 업뎃안에 넣고 돌릴것
    /// </summary>
    virtual protected void CheckGround()
    {
        Vector2 temp = new Vector2(this.transform.position.x, this.transform.position.y + feetPosOffset.y);
        isGrounded = Physics2D.OverlapBox(temp, feetSize, 0f, moveableLayer);
    }


    [Header("디버그 용 !읽기만 하세요!")]
    [Header("오브젝트 컴포넌트")]
    [Tooltip("이 오브젝트 rigidbody2D")]
    [SerializeField] protected Rigidbody2D rigid = null;

    [Tooltip("이 오브젝트 collider")]
    [SerializeField] protected Collider2D col = null;

    [Tooltip("바닥 충돌판정할 위치")]
    [SerializeField] protected Vector2 feetPosOffset;

    [Header("땅 판정")]
    [Tooltip("땅 판정 bool")]
    [SerializeField] protected bool isGrounded = false;

    [Header("점프키를 계속 누르고 있는지")]
    public bool isLongJump = false;

    //[Tooltip("현재 이동 예정 방향")]
    //protected Vector2 curTargetPos;

    //[property: Tooltip("현재 이동 예정 방향_읽기 전용")]
    //public Vector2 CurTargetPos
    //{
    //    get { return curTargetPos; }
    //    protected set { curTargetPos = value; }
    //}
}
