using UnityEngine;

public class PlayerMove : BaseMove
{
    [Header("---------------------------------------------------------------------------------------")]
    [Header("�÷��̾� ����")]
    [SerializeField]
    private Rigidbody2D hookRigid;
    private HookMove hookMove;
    private PlayerController playerController;

    [Header("---------------------------------------------------------------------------------------")]
    [Header("����")]
    [Tooltip("���� Ƚ��")]
    [SerializeField] protected Definition.IStat maxJumpCount = new Definition.IStat(2);
    public Definition.IStat MaxJumpCount { get { return maxJumpCount; } }
    [Tooltip("���� ����")]
    [SerializeField] protected Definition.FStat wireJumpForce = new Definition.FStat(3f);
    public Definition.FStat WireJumpForce { get { return wireJumpForce; } }
    [Tooltip("���̾� ����")]
    [SerializeField] protected Definition.FStat swingJumpForce = new Definition.FStat(3f);
    public Definition.FStat SwingJumpForce { get { return swingJumpForce; } }

    protected override void Awake()
    {
        base.Awake();
        playerController = (PlayerController)baseController;
        hookRigid.TryGetComponent(out hookMove);
    }

    public override void MoveToX(float x)
    {
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

    [SerializeField] private int remainedJumpCount = 0;
    protected override void JumpAdditive()
    {
        if (isGrounded)
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


            if (playerController.IsWireTensioned && !IsGrounded)
            {
                SwingJump();
            }
        }
    }

    #region Wire
    protected void LateUpdate()
    {
        WirePhysicsCal();
    }
    private void WirePhysicsCal()
    {
        if (playerController.IsHookAnchored && !playerController.IsHookAnchoredAtOBJ)
        {
            Vector2 dir = ((Vector2)(hookRigid.transform.position - this.transform.position)).normalized;
            float dis = ((Vector2)(hookRigid.transform.position - this.transform.position)).magnitude;

            if (dis >= hookMove.CurWireLength)
            {
                playerController.IsWireTensioned = true;

                Vector2 playerSpeed = rigid.linearVelocity;
                if (Vector2.Dot(playerSpeed, dir) <= 0)
                {
                    Vector2 newPlayerSpeed = playerSpeed - Vector2.Dot(playerSpeed, dir) * dir;
                    rigid.linearVelocity = newPlayerSpeed;
                }
                rigid.transform.position = hookRigid.transform.position - (Vector3)(dir * hookMove.CurWireLength);
            }
            else
            {
                playerController.IsWireTensioned = false;
            }
        }
    }


    public void WireJump()
    {
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
