using Definition;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class BaseMove : MonoBehaviour
{
    [Header("�̼� ��ġ")]
    [Tooltip("�̼�")]
    [SerializeField] protected FStat moveSpeed = new FStat(5f);
    public FStat MoveSpeed { get { return moveSpeed; } }
    [Tooltip("�ְ� �ӵ� ���� �ð�")]
    [SerializeField] protected float accelerateTime = 0.1f;

    [Tooltip("����")]
    [SerializeField] protected FStat jumpForce = new FStat(3f);
    public FStat JumpForce { get { return jumpForce; } }

    [Header("�߷µ�")]
    [Tooltip("�⺻ �߷�")]
    [SerializeField] protected float baseGravity = 10f;
    public float BaseGravity { get { return baseGravity; } set { baseGravity = value; } }

    [Tooltip("���� ��½� �߷�")]
    [SerializeField] protected float jumpGravity = 5f;
    public float JumpGravity { get { return jumpGravity; } set { jumpGravity = value; } }


    [Header("�� �ٴ�")]
    [Tooltip("�� ���� ũ��")]
    [SerializeField] protected Vector2 feetSize = new Vector2(1f, 0.1f);
    [Tooltip("�� ���� x ��ǥ ������")]
    [SerializeField] protected float feetPosOffsetX = 0f;

    [Header("��Ÿ ���")]

    [Tooltip("�̵� ���� ���̾�")]
    [SerializeField] protected LayerMask moveableLayer;
    public LayerMask MoveableLayer { get { return moveableLayer; } set { moveableLayer = value; } }

    // [SerializeField] protected BoxCollider2D groundCollider = null;


    protected BaseController baseController;
    virtual protected void Awake()
    {
        TryGetComponent<BaseController>(out baseController);

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
            float localMinY = col.bounds.min.y - transform.position.y;
            feetPosOffset = new Vector2(feetPosOffsetX, localMinY);

            #region ���ô��� ����
            // groundCollider = this.gameObject.AddComponent<BoxCollider2D>();

            // groundCollider.size = groundColSize;

            //Vector2 groundColPos = new Vector2(col.bounds.center.x, col.bounds.min.y);

            //feetPosOffset = groundColPos - (Vector2)this.transform.position;

            // groundCollider.offset = groundColPos; 
            #endregion
        }
    }

    virtual protected void Update()
    {
        CheckGround();
        JumpAdditive();
    }

    virtual protected void FixedUpdate()
    {
        JumpHeightUpdate();
    }

    /// <summary>
    /// �̵� ���. ����μ� target�ް� target�� �̼� ����������, �ٲ� �� �ִ�.
    /// </summary>
    /// <param name="dir"> �̵��ϰ��� �ϴ� ����.</param>
    virtual public void MoveTo(Vector3 dir)
    {
        //curTargetPos = target;

        rigid.linearVelocity = dir * moveSpeed.FinalStat();
    }
    virtual public void MoveTo(Transform target)
    {
        rigid.linearVelocity = (target.position - this.transform.position) * moveSpeed.FinalStat();
    }
    virtual public void MoveTo(Vector3 dir, float speed)
    {
        rigid.linearVelocity = dir * speed;
    }
    virtual public void MoveTo(Transform target, float speed)
    {
        rigid.linearVelocity = (target.position - this.transform.position) * speed;
    }

    protected float curVelocity;
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

    [Header("����")]
    [Tooltip("���� ���Է� �ð�")]
    [SerializeField] protected float jumpBufferTime = 0;
    protected float jumpBufferTimer = -1;
    public float JumpBufferTimer { get { return jumpBufferTime; } }

    [Tooltip("���� ���� �ð�")]
    [SerializeField] protected float coyoteTime = 0;
    protected float coyoteTimer = -1;
    public float CoyoteTimer { get { return coyoteTimer; } }

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
    /// �� üũ �Լ�. �����ȿ� �ְ� ������
    /// </summary>
    virtual protected void CheckGround()
    {
        Vector2 temp = new Vector2(this.transform.position.x, this.transform.position.y + feetPosOffset.y);
        isGrounded = Physics2D.OverlapBox(temp, feetSize, 0f, moveableLayer) != null;

        // drawline��� -- �浹���� ���̰�
        Debug.DrawLine(
            temp + Vector2.left * feetSize.x * 0.5f,
            temp + Vector2.right * feetSize.x * 0.5f,
            isGrounded ? Color.green : Color.red
        );
        Debug.DrawLine(
            temp + Vector2.up * feetSize.y * 0.5f,
            temp + Vector2.down * feetSize.y * 0.5f,
            isGrounded ? Color.green : Color.red
        );
    }


    [Header("����� �� !�б⸸ �ϼ���!")]
    [Header("������Ʈ ������Ʈ")]
    [Tooltip("�� ������Ʈ rigidbody2D")]
    [SerializeField] protected Rigidbody2D rigid = null;

    [Tooltip("�� ������Ʈ collider")]
    [SerializeField] protected Collider2D col = null;

    [Tooltip("�ٴ� �浹������ ��ġ")]
    [SerializeField] protected Vector2 feetPosOffset;

    [Header("�� ����")]
    [Tooltip("�� ���� bool")]
    [SerializeField] protected bool isGrounded = false;
    public bool IsGrounded { get { return isGrounded; } }

    [Header("����Ű�� ��� ������ �ִ���")]
    public bool isLongJump = false;

    //[Tooltip("���� �̵� ���� ����")]
    //protected Vector2 curTargetPos;

    //[property: Tooltip("���� �̵� ���� ����_�б� ����")]
    //public Vector2 CurTargetPos
    //{
    //    get { return curTargetPos; }
    //    protected set { curTargetPos = value; }
    //}
}
