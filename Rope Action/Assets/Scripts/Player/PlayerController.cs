using UnityEngine;
using System;

public class PlayerController : BaseController
{
    [SerializeField]
    private KeyCode jumpKeyCode = KeyCode.Space;
    [SerializeField]
    private KeyCode leftKeyCode = KeyCode.A;
    [SerializeField]
    private KeyCode rightKeyCode = KeyCode.D;
    [SerializeField]
    private KeyCode hookReturnKeyCode = KeyCode.LeftShift;

    [SerializeField]
    private WirePhysics wirePhysics;
    [SerializeField]
    private bool isHookAnchored = false;
    public bool IsHookAnchored { get { return isHookAnchored; } set { isHookAnchored = value; } }
    [SerializeField]
    private bool isWireTensioned = false;
    public bool IsWireTensioned { get { return isWireTensioned; } set { isWireTensioned = value; } }

    protected override void Awake()
    {
        base.Awake();
    }
    private void Update()
    {
        UpdateMove();
        UpdateWire();
    }

    #region Move
    public event Action OnJumpStart;
    public event Action<int> OnWalking;
    private void UpdateMove()
    {
        if (isHookAnchored == false)
        {
            if (Input.GetKeyDown(jumpKeyCode))
            {
                baseMove.Jump();
                baseMove.isLongJump = true;
            }
            else if (Input.GetKeyUp(jumpKeyCode))
            {
                baseMove.isLongJump = false;
            }
        }

        int x = 0;
        if (Input.GetKey(leftKeyCode))
        {
            x -= 1;
        }
        if (Input.GetKey(rightKeyCode))
        {
            x += 1;
        }

        if (x != 0 && baseMove.IsGrounded)
        {
            OnWalking?.Invoke(x);
        }

        if (x != 0)
            baseMove.MoveToX(x);
        else if (!IsWireTensioned)
            baseMove.MoveToX(x);

        //baseMove.MoveToX(Input.GetAxis("Horizontal"));
    }
    public void InvokeOnJumpStart()
    {
        OnJumpStart?.Invoke();
    }
    #endregion

    #region Wire
    private void UpdateWire()
    {
        if (isHookAnchored == true)
        {
            if (Input.GetKey(hookReturnKeyCode))
            {
                wirePhysics.ReturnHookShot();
            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            wirePhysics.FireHookShot();
        }
        if (Input.GetMouseButton(1))
        {
            wirePhysics.ZipUpWire();
        }
    }
    #endregion
}
