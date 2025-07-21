using UnityEngine;
using System;

public class PlayerController : BaseController
{
    [SerializeField]
    private HookMove hookMove;

    [Header("---------------------------------------------------------------------------------------")]
    [Header("키 코드")]
    [SerializeField]
    private KeyCode jumpKeyCode = KeyCode.Space;
    [SerializeField]
    private KeyCode leftKeyCode = KeyCode.A;
    [SerializeField]
    private KeyCode rightKeyCode = KeyCode.D;
    [SerializeField]
    private KeyCode hookReturnKeyCode = KeyCode.LeftShift;


    [Header("---------------------------------------------------------------------------------------")]
    [Header("ECT")]
    [SerializeField]
    private Camera mainCamera;


    [Header("---------------------------------------------------------------------------------------")]
    [Header("확인용")]

    [SerializeField]
    private bool isHookAnchored = false;
    public bool IsHookAnchored { get { return isHookAnchored; } set { isHookAnchored = value; } }

    [SerializeField]
    private bool isWireTensioned = false;
    public bool IsWireTensioned { get { return isWireTensioned; } set { isWireTensioned = value; } }

    [SerializeField]
    private Vector2 mouseDir;
    public Vector2 MouseDir { get { return mouseDir; } }


    protected PlayerMove playerMove;
    protected override void Awake()
    {
        base.Awake();
        playerMove = (PlayerMove)baseMove;
    }
    private void Update()
    {
        InputController();
    }

    private void InputController()
    {
        mouseDir = ((Vector2)(mainCamera.ScreenToWorldPoint(Input.mousePosition) - this.transform.position)).normalized;
        UpdateMove();
        UpdateWire();

        //--------------------------------------------------------------------------------------------------------------------------

        //{

        //}

        //bool isCommonState = true;

        //if (IsHookAnchored)
        //{
        //    isCommonState = false;
        //}

        //if (IsWireTensioned)
        //{
        //    isCommonState = false;
        //}

        //if (isCommonState)
        //{ 

        //}
    }


    #region Move
    public event Action OnJumpStart;
    public event Action<int> OnWalking;
    private void UpdateMove()
    {
        if (Input.GetKeyDown(jumpKeyCode))
        {
            playerMove.Jump();
            playerMove.isLongJump = true;
        }
        else if (Input.GetKeyUp(jumpKeyCode))
        {
            playerMove.isLongJump = false;
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

        if (x != 0 && playerMove.IsGrounded)
        {
            OnWalking?.Invoke(x);
        }

        if (x != 0)
            playerMove.MoveToX(x);
        else if (!IsWireTensioned)
            playerMove.MoveToX(x);

        //playerMove.MoveToX(Input.GetAxis("Horizontal"));
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
                hookMove.ReturnHookShot();
            }
            if (Input.GetMouseButtonDown(1))
            {
                playerMove.WireJump();
            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            hookMove.FireHookShot();
        }
    }
    #endregion
}
