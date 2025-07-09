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



    [Header("��Ÿ ���")]

    [Tooltip("�̵� ���� ���̾�")]
    [SerializeField] protected LayerMask moveableLayer;
    public LayerMask MoveableLayer { get { return moveableLayer; } set { moveableLayer = value; } }

    [Tooltip("�� ���� ũ��")]
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
    /// �̵� ���. ����μ� target�ް� target�� �̼� ����������, �ٲ� �� �ִ�.
    /// </summary>
    /// <param name="target"> �̵��ϰ��� �ϴ� ����.</param>
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

    [Header("����")]
    [Tooltip("���� ���Է� �ð�")]
    [SerializeField] private float jumpBufferTime = 0;
    private float jumpBufferTimer = -1;

    [Tooltip("���� ���� �ð�")]
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
    /// �� üũ �Լ�. �����ȿ� �ְ� ������
    /// </summary>
    virtual protected void CheckGround()
    {
        Vector2 temp = new Vector2(this.transform.position.x, this.transform.position.y + feetPosOffset.y);
        isGrounded = Physics2D.OverlapBox(temp, feetSize, 0f, moveableLayer);
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
