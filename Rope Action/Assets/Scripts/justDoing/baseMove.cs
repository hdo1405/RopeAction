using Definition;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class BaseMove : MonoBehaviour
{
    [Header("�̼� ��ġ")]
    [Tooltip("�̼�")]
    [SerializeField] protected FStat moveSpeed = new FStat(5f);

    [Tooltip("����")]
    [SerializeField] protected FStat jumpForce = new FStat(3f);

    [Header("�߷µ�")]
    [Tooltip("�⺻ �߷�")]
    [SerializeField] protected float baseGravity = 9f;

    [Tooltip("���� �߷�")]
    [SerializeField] protected float airborneGravity = 15f;

    [Tooltip("�̵� ���� ���̾�")]
    [SerializeField] protected LayerMask moveableLayer;

    [Header("����� �� !�б⸸ �ϼ���!")]
    [Header("������Ʈ ������Ʈ")]
    [Tooltip("�� ������Ʈ rigidbody2D")]
    [SerializeField] protected Rigidbody2D rigid = null;

    [Tooltip("�� ������Ʈ collider")]
    [SerializeField] protected Collider2D col = null;


    [Tooltip("�� ���� ũ��")]
    [SerializeField] protected Vector2 feetSize = new Vector2(1f, 0.1f);
    // [SerializeField] protected BoxCollider2D groundCollider = null;

    [Tooltip("�ٴ� �浹������ ��ġ")]
    [SerializeField] protected Vector3 feetPosOffset;

    [Header("�� ����")]
    [Tooltip("�� ���� bool")]
    [SerializeField] protected bool isGrounded = false;

    [Tooltip("���� �̵� ���� ����")]
    protected Vector3 curTargetPos;

    [property:Tooltip("���� �̵� ���� ����_�б� ����")]
    public Vector3 CurTargetPos 
    { 
        get { return curTargetPos; } 
        protected set { curTargetPos = value; } 
    }

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

            Vector3 groundColPos = new Vector3(col.bounds.center.x, col.bounds.min.y, col.bounds.center.z);

            feetPosOffset = groundColPos;

            // groundCollider.offset = groundColPos;
        }
    }

    virtual protected void FixedUpdate()
    {
        CheckGround();
    }

    /// <summary>
    /// �̵� ���. ����μ� target�ް� target�� �̼� ����������, �ٲ� �� �ִ�.
    /// </summary>
    /// <param name="target"> �̵��ϰ��� �ϴ� ����.</param>
    virtual public void MoveTo(Vector3 target)
    {
        curTargetPos = target;

        rigid.linearVelocity = curTargetPos * moveSpeed.FinalStat();
    }

    /// <summary>
    /// �� üũ �Լ�. �����ȿ� �ְ� ������
    /// </summary>
    virtual protected void CheckGround()
    {
        Vector2 temp = new Vector2(this.transform.position.x, this.transform.position.y + feetPosOffset.y);
        isGrounded = Physics2D.OverlapBox(temp, feetSize, 0f, moveableLayer);
    }
}
