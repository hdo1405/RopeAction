using UnityEngine;

public class PlayerMove : BaseMove
{
    [Header("---------------------------------------------------------------------------------------")]
    [Header("플레이어 무브")]
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private Rigidbody2D hookRigid;
    private HookMove hookMove;
    private PlayerController playerController;
    [SerializeField] protected Vector2 wallDetectorSize = new Vector2(0.2f, 0.1f);
    protected Vector2 wallDetectorPos;
    protected bool isAttatchedWall = false;

    [Header("---------------------------------------------------------------------------------------")]
    #region Stat
    [Header("스텟")]

    [Tooltip("달리기 속도 배율")]
    [SerializeField] protected float runSpeedScale = 1.2f;

    [Tooltip("벽타기 속도")]
    [SerializeField] protected Definition.FStat climbSpeed = new Definition.FStat(5f);

    [Tooltip("점프 횟수")]
    [SerializeField] protected Definition.IStat maxJumpCount = new Definition.IStat(2);
    public Definition.IStat MaxJumpCount { get { return maxJumpCount; } }

    [Tooltip("스윙 점프")]
    [SerializeField] protected Definition.FStat wireJumpForce = new Definition.FStat(3f);
    public Definition.FStat WireJumpForce { get { return wireJumpForce; } }

    [Tooltip("와이어 점프")]
    [SerializeField] protected Definition.FStat swingJumpForce = new Definition.FStat(3f);
    public Definition.FStat SwingJumpForce { get { return swingJumpForce; } } 
    #endregion

    protected override void Awake()
    {
        base.Awake();

        playerController = (PlayerController)baseController;
        hookRigid.TryGetComponent(out hookMove);

        if (col != null)
        {
            float localMidY = col.bounds.center.y - transform.position.y;
            float localEdgeX = col.bounds.max.x- transform.position.x;
            wallDetectorPos = new Vector2(localEdgeX, localMidY);
        }
    }
    protected override void Update()
    {
        base.Update();
        CheckWall();
    }


    public override void MoveToX(float x)
    {
        bool isRun = false;
        if (Mathf.Abs(x) > 1) isRun = true;
        if (x != 0) x = Mathf.Sign(x);
        else
        {
            if (Mathf.Abs(rigid.linearVelocityX) < moveSpeed.FinalStat())
            {
                float newSpeed = Mathf.SmoothDamp(rigid.linearVelocityX, 0, ref curVelocity, accelerateTime);
                rigid.linearVelocityX = newSpeed;
            }
            return;
        }

        if (x > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (x < 0)
        {
            spriteRenderer.flipX = true;
        }

        if (x == Mathf.Sign(rigid.linearVelocityX))
        {
            if (Mathf.Abs(rigid.linearVelocityX) < moveSpeed.FinalStat())
            {
                float newSpeed = Mathf.SmoothDamp(rigid.linearVelocityX, x * moveSpeed.FinalStat(), ref curVelocity, accelerateTime);
                if (isRun) newSpeed *= runSpeedScale;
                rigid.linearVelocityX = newSpeed;
            }
        }
        else
        {
            float newSpeed = Mathf.SmoothDamp(rigid.linearVelocityX, x * moveSpeed.FinalStat(), ref curVelocity, accelerateTime);
            if (isRun) newSpeed *= runSpeedScale;
            rigid.linearVelocityX = newSpeed;
        }
    }


    [SerializeField] private int remainedJumpCount = 0;
    protected override void JumpAdditive()
    {
        if (isGrounded || isAttatchedWall)
        {
            remainedJumpCount = maxJumpCount.FinalStat() - 1;
            coyoteTimer = coyoteTime;
        }
        else coyoteTimer -= Time.deltaTime;

        if (jumpBufferTimer > 0) jumpBufferTimer -= Time.deltaTime;

        if (jumpBufferTimer > 0)
        {
            if (coyoteTimer > 0)
            {
                rigid.linearVelocityY = jumpForce.FinalStat();
                jumpBufferTimer = 0;
                coyoteTimer = 0;
                playerController?.InvokeOnJumpStart();
            }
            else if (remainedJumpCount > 0)
            {
                rigid.linearVelocityY = jumpForce.FinalStat();
                remainedJumpCount -= 1;
                jumpBufferTimer = 0;
                coyoteTimer = 0;
                //Debug.Log("jump: " + remainedJumpCount);
                playerController?.InvokeOnJumpStart();
            }


            if (playerController.IsWireTensioned && !IsGrounded && !playerController.IsHookAnchoredAtOBJ)
            {
                SwingJump();
            }
        }
    }

    protected override void JumpHeightUpdate()
    {
        if (isAttatchedWall) return;
        if (isLongJump && rigid.linearVelocityY >= 0)
        {
            rigid.gravityScale = jumpGravity;
        }
        else
        {
            rigid.gravityScale = baseGravity;
        }
    }

    protected void CheckWall()
    {
        if (rigid.linearVelocityX == 0)
        {
            isAttatchedWall = false;
            return;
        }

        if (Mathf.Sign(wallDetectorPos.x) != Mathf.Sign(rigid.linearVelocityX))
        {
            wallDetectorPos.x *= -1;
        }

        Vector2 temp = (Vector2)this.transform.position + wallDetectorPos;
        isAttatchedWall = Physics2D.OverlapBox(temp, feetSize, 0f, moveableLayer) != null;

        if (isAttatchedWall)
        {
            if (rigid.linearVelocityY > climbSpeed.FinalStat())
                isAttatchedWall = false;
        }

        if (isAttatchedWall)
        {
            rigid.gravityScale = 0;
        }
        else
        {
            rigid.gravityScale = baseGravity;
        }
    }

    float curClimbVelocity = 0;
    public void WallClimb(float y)
    {
        if (isAttatchedWall)
        {
            if (y != 0) y = Mathf.Sign(y);

            float newSpeed = Mathf.SmoothDamp(rigid.linearVelocityY, y * climbSpeed.FinalStat(), ref curClimbVelocity, accelerateTime);
            rigid.linearVelocityY = newSpeed;
        }
    }

    #region Wire
    protected void LateUpdate()
    {
        WirePhysicsCal();
    }
    private void WirePhysicsCal()
    {
        if (!playerController.IsHookAnchored) return;

        float dis = ((Vector2)(hookRigid.transform.position - this.transform.position)).magnitude;

        if (dis >= hookMove.CurWireLength) playerController.IsWireTensioned = true;
        else playerController.IsWireTensioned = false;

        if (playerController.IsHookAnchored && !playerController.IsHookAnchoredAtOBJ)
        {
            Vector2 dir = ((Vector2)(hookRigid.transform.position - this.transform.position)).normalized;

            if (playerController.IsWireTensioned)
            {
                Vector2 playerSpeed = rigid.linearVelocity;
                if (Vector2.Dot(playerSpeed, dir) <= 0)
                {
                    Vector2 newPlayerSpeed = playerSpeed - Vector2.Dot(playerSpeed, dir) * dir;
                    rigid.linearVelocity = newPlayerSpeed;
                }
                rigid.transform.position = hookRigid.transform.position - (Vector3)(dir * hookMove.CurWireLength);
            }
        }
    }


    public void WireJump()
    {
        if (!playerController.IsHookAnchored) return;
        Vector2 dir = ((Vector2)(hookRigid.transform.position - this.transform.position)).normalized;

        rigid.linearVelocity += dir * wireJumpForce.FinalStat();

        hookMove.ReturnHookShot();

        //hookMove.CurWireLength -= Time.deltaTime * hookMove.PullingForce.FinalStat();

        //if (hookMove.CurWireLength <= 0)
        //{
        //    hookMove.CurWireLength = 0;
        //    rigid.linearVelocity = Vector2.zero;
        //    rigid.transform.position = hookRigid.transform.position;
        //}


        //Vector2 dir = ((Vector2)(hookRigid.transform.position - this.transform.position)).normalized;
        //float dis = ((Vector2)(hookRigid.transform.position - this.transform.position)).magnitude;

        //if (dis < Time.deltaTime * hookMove.PullingForce.FinalStat())
        //{
        //    rigid.linearVelocity = Vector2.zero;
        //    rigid.transform.position = hookRigid.transform.position;
        //    return;
        //}

        //rigid.linearVelocity = dir * hookMove.PullingForce.FinalStat();

        //hookMove.CurWireLength = (this.transform.position - hookRigid.transform.position).magnitude;
    }

    public void SwingJump()
    {
        if (!playerController.IsHookAnchored) return;
        //Debug.Log("swing start");
        float wireJumpScale = rigid.linearVelocity.magnitude / moveSpeed.FinalStat();
        //Debug.Log("wireJumpSclae: " + wireJumpScale);

        hookMove.ReturnHookShot();
        remainedJumpCount = maxJumpCount.FinalStat() - 1;
        //Debug.Log("swing: "+remainedJumpCount);

        if (wireJumpScale < 1) return;
        if (rigid.linearVelocityY < 0) return;

        Vector2 newVelocity = rigid.linearVelocity.normalized;
        newVelocity *= swingJumpForce.FinalStat() * wireJumpScale;
        rigid.linearVelocity = newVelocity;

        //Vector2 newVelocity = rigid.linearVelocity.normalized;
        //newVelocity *= jumpForce.FinalStat();
        //rigid.linearVelocity = newVelocity;
    }
    #endregion
}
