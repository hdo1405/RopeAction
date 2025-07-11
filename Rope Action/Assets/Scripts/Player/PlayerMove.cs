using UnityEngine;

public class PlayerMove : BaseMove
{
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

    protected override void JumpAdditive()
    {
        if (isGrounded) coyoteTimer = coyoteTime;
        else coyoteTimer -= Time.deltaTime;

        if (jumpBufferTimer > 0) jumpBufferTimer -= Time.deltaTime;

        if (jumpBufferTimer > 0 && coyoteTimer > 0)
        {
            rigid.linearVelocityY = jumpForce.FinalStat();
            jumpBufferTimer = 0;
            coyoteTimer = 0;
            ((PlayerController)baseController)?.InvokeOnJumpStart();
        }
    }
}
