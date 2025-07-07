using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class BaseMove : MonoBehaviour
{
    [Header("이속 수치")]
    [Tooltip("이속")]
    [SerializeField] protected float moveSpeed = 5f;

    [Tooltip("점프")]
    [SerializeField] protected float jumpForce = 3f;

    [Header("중력들")]
    [Tooltip("기본 중력")]
    [SerializeField] protected float baseGravity = 9f;

    [Tooltip("공중 중력")]
    [SerializeField] protected float airborneGravity = 15f;

    [Tooltip("이동 가능 레이어")]
    [SerializeField] protected LayerMask moveableLayer;

    [Header("디버그 용 !읽기만 하세요!")]
    [Header("오브젝트 컴포넌트")]
    [Tooltip("이 오브젝트 rigidbody2D")]
    [SerializeField] protected Rigidbody2D rigidbody = null;

    [Tooltip("이 오브젝트 collider")]
    [SerializeField] protected Collider2D collider = null;


    [Tooltip("발 판정 크기")]
    [SerializeField] protected Vector2 feetSize = new Vector2(1f, 0.1f);
    // [SerializeField] protected BoxCollider2D groundCollider = null;

    [Tooltip("바닥 충돌판정할 위치")]
    [SerializeField] protected Vector3 feetPosOffset;

    [Header("땅 판정")]
    [Tooltip("땅 판정 bool")]
    [SerializeField] protected bool isGrounded = false;

    [Tooltip("현재 이동 예정 방향")]
    protected Vector3 curTargetPos;

    [property:Tooltip("현재 이동 예정 방향_읽기 전용")]
    public Vector3 CurTargetPos 
    { 
        get { return curTargetPos; } 
        protected set { curTargetPos = value; } 
    }

    virtual protected void Awake()
    {
        if (rigidbody == null)
        {
            TryGetComponent<Rigidbody2D>(out rigidbody);
        }

        if (collider == null)
        {
            TryGetComponent<Collider2D>(out collider);
        }

        if (collider != null)
        {
            // groundCollider = this.gameObject.AddComponent<BoxCollider2D>();

            // groundCollider.size = groundColSize;

            Vector3 groundColPos = new Vector3(collider.bounds.center.x, collider.bounds.min.y, collider.bounds.center.z);

            feetPosOffset = groundColPos;

            // groundCollider.offset = groundColPos;
        }
    }

    virtual protected void FixedUpdate()
    {
        CheckGround();
    }

    /// <summary>
    /// 이동 담당. 현재로선 target받고 target에 이속 곱해주지만, 바뀔 수 있다.
    /// </summary>
    /// <param name="target"> 이동하고자 하는 방향.</param>
    virtual public void MoveTo(Vector3 target)
    {
        curTargetPos = target;

        rigidbody.linearVelocity = curTargetPos * moveSpeed;
    }

    /// <summary>
    /// 땅 체크 함수. 업뎃안에 넣고 돌릴것
    /// </summary>
    virtual protected void CheckGround()
    {
        Vector2 temp = new Vector2(this.transform.position.x, this.transform.position.y + feetPosOffset.y);
        isGrounded = Physics2D.OverlapBox(temp, feetSize, 0f, moveableLayer);
    }
}
