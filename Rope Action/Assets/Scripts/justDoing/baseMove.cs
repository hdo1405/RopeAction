using System.Runtime.ExceptionServices;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class baseMove : MonoBehaviour
{
    [Header("�̼� ��ġ")]
    [Tooltip("�̼�")]
    [SerializeField] protected float moveSpeed = 5f;

    [Tooltip("����")]
    [SerializeField] protected float jumpForce = 3f;

    [Header("�߷µ�")]
    [Tooltip("�⺻ �߷�")]
    [SerializeField] protected float baseGravity = 9f;

    [Tooltip("���� �߷�")]
    [SerializeField] protected float airborneGravity = 15f;

    [Header("������Ʈ ������Ʈ")]
    [Tooltip("�� ������Ʈ rigidbody2D")]
    [SerializeField] protected Rigidbody2D myRB = null;

    [Tooltip("�� ������Ʈ collider")]
    [SerializeField] protected Collider2D myCol = null;

    [Header("�� ����")]
    [Tooltip("�� ���� bool")]
    [SerializeField] protected bool isGrounded = false;

    [Tooltip("�� ���� ũ��")]
    [SerializeField] protected Vector2 groundColSize = new Vector2(1f, 0.1f);

    [Tooltip("�̵� ���� ���̾�")]
    [SerializeField] protected LayerMask moveableLayer;

    [Tooltip("�ٴ� �浹������ ��ġ")]
    [SerializeField] protected Vector3 feetPosOffset;

    // [SerializeField] protected BoxCollider2D groundCollider = null;

    [Tooltip("���� �̵� ���� ����")]
    protected Vector3 curTargetPos;

    [property:Tooltip("���� �̵� ���� ����_�б� ����")]
    public Vector3 CurTargetPos { get { return curTargetPos; } protected set { curTargetPos = value; } }

    virtual protected void Start()
    {
        if (myRB == null)
        {
            TryGetComponent<Rigidbody2D>(out myRB);
        }

        if (myCol == null)
        {
            TryGetComponent<Collider2D>(out myCol);
        }

        if (myCol != null)
        {
            // groundCollider = this.gameObject.AddComponent<BoxCollider2D>();

            // groundCollider.size = groundColSize;

            Vector3 groundColPos = new Vector3(myCol.bounds.center.x, myCol.bounds.min.y, myCol.bounds.center.z);

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

        myRB.linearVelocity = curTargetPos * moveSpeed;
    }

    /// <summary>
    /// �� üũ �Լ�. �����ȿ� �ְ� ������
    /// </summary>
    virtual protected void CheckGround()
    {
        Vector2 temp = new Vector2(this.transform.position.x, this.transform.position.y + feetPosOffset.y);
        isGrounded = Physics2D.OverlapBox(temp, groundColSize, 0f, moveableLayer);
    }
}
