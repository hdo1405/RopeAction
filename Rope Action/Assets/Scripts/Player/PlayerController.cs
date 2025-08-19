using UnityEngine;
using System;

public class PlayerController : BaseController
{
    [SerializeField]
    private HookMove hookMove;
    [SerializeField]
    private CrossHairRenderer crossHairRenderer;

    [Header("---------------------------------------------------------------------------------------")]
    [Header("Ű �ڵ�")]
    [SerializeField]
    private KeyCode jumpKeyCode = KeyCode.Space;
    [SerializeField]
    private KeyCode runKeyCode = KeyCode.LeftControl;
    [SerializeField]
    private KeyCode leftKeyCode = KeyCode.A;
    [SerializeField]
    private KeyCode rightKeyCode = KeyCode.D;
    [SerializeField]
    private KeyCode upKeyCode = KeyCode.W;
    [SerializeField]
    private KeyCode downKeyCode = KeyCode.S;
    [SerializeField]
    private KeyCode hookReturnKeyCode = KeyCode.R;
    [SerializeField]
    private KeyCode wireJumpKeyCode = KeyCode.LeftShift;


    [Header("---------------------------------------------------------------------------------------")]
    [Header("ECT")]
    [SerializeField]
    private Camera mainCamera;


    [Header("---------------------------------------------------------------------------------------")]
    [Header("Ȯ�ο�")]

    [SerializeField]
    private bool isHookAnchored = false;
    public bool IsHookAnchored { get { return isHookAnchored; } set { isHookAnchored = value; } }
    [SerializeField]
    private bool isHookAnchoredAtOBJ = false;
    public bool IsHookAnchoredAtOBJ { get { return isHookAnchoredAtOBJ; } set { isHookAnchoredAtOBJ = value; } }

    [SerializeField]
    private bool isWireTensioned = false;
    public bool IsWireTensioned { get { return isWireTensioned; } set { isWireTensioned = value; } }

    [SerializeField]
    private Vector2 mouseDir;
    public Vector2 MouseDir { get { return mouseDir; } }
    public float FloatMouseDir 
    {
        get 
        {
            float angle = Mathf.Atan2(mouseDir.y, mouseDir.x) * Mathf.Rad2Deg;
            return angle;
        }
    }


    protected PlayerMove playerMove;
    protected override void Awake()
    {
        base.Awake();
        playerMove = (PlayerMove)baseMove;
    }
    private void Update()
    {
        InputManage();
    }

    #region Events
    public event Action OnJumpStart;
    public void InvokeOnJumpStart()
    {
        OnJumpStart?.Invoke();
    }
    public event Action<int> OnWalking;
    #endregion

    private void InputManage()
    {
        CheckAlways();

        bool isCommonState = true;

        //������ ��������
        if (IsHookAnchored)
        {
            DuringHookAnchored();

            if (IsWireTensioned)
            {
                //���� ������ ��
                DuringWireTensioned();
            }

            isCommonState = false;
        }

        //Ư�̻����� ������
        if (isCommonState)
        {
            WhenCommon();
        }

        ////�������� ��
        //if (Input.GetMouseButton(1))
        //{
        //    DuringAiming();
        //}
        ////�������� �ƴ� ��
        //else
        //{
        //    DuringNotAiming();
        //}

        //UpdateMove();
        //UpdateWire();
    }

    private void DuringHookAnchored()
    {
        //if (Input.GetKey(hookReturnKeyCode))
        if (Input.GetKey(wireJumpKeyCode))
        {
            //if (isHookAnchoredAtOBJ)
            //{
            //    hookMove.PullingAnchoredOBJ();
            //}
            //else
            {
                playerMove.WireJump();
            }
        }
    }
    private void DuringWireTensioned()
    {
    }
    private void DuringAiming()
    {
        if (Input.GetMouseButtonDown(0))
        {
            hookMove.FireHookShot();
        }
    }
    private void DuringNotAiming()
    {
    }
    private void WhenCommon()
    {
        if (Input.GetMouseButtonDown(1))
        {
            hookMove.FireHookShot();
        }
    }

    private void CheckAlways()
    {
        #region Move
        mouseDir = ((Vector2)(mainCamera.ScreenToWorldPoint(Input.mousePosition) - this.transform.position)).normalized;

        if (Input.GetKeyDown(jumpKeyCode))
        {
            playerMove.Jump();
            playerMove.isLongJump = true;
        }
        else if (Input.GetKeyUp(jumpKeyCode))
        {
            playerMove.isLongJump = false;
        }

        #region Walk & Run
        int x = 0;
        int isRun = 1;
        if (Input.GetKey(leftKeyCode))
        {
            x -= 1;
        }
        if (Input.GetKey(rightKeyCode))
        {
            x += 1;
        }
        if (Input.GetKey(runKeyCode))
        {
            isRun = 2;
        }

        if (x != 0 && playerMove.IsGrounded)
        {
            OnWalking?.Invoke(x);
        }

        if (x != 0)
        {
            playerMove.MoveToX(isRun * x);
        }
        else if (!IsWireTensioned)
        {
            playerMove.MoveToX(x);
        }
        #endregion

        int y = 0;
        if (Input.GetKey(downKeyCode))
        {
            y -= 1;
        }
        if (Input.GetKey(upKeyCode))
        {
            y += 1;
        }

        playerMove.WallClimb(y);
        #endregion

        if (Input.GetMouseButtonUp(1))
        {
            hookMove.ReturnHookShot();
        }

        if (Input.GetMouseButton(0))
        {
            ((PlayerAttack)baseAttack).Attack();
        }

        //if (Input.GetMouseButtonDown(0))
        //{
        //    crossHairRenderer.showCrossHair = true;
        //}
        //else if (Input.GetMouseButtonUp(0))
        //{
        //    crossHairRenderer.showCrossHair = false;
        //}
    }

    //���ô��� ������
    //#region Move
    //public event Action OnJumpStart;
    //public event Action<int> OnWalking;
    //private void UpdateMove()
    //{
    //    if (Input.GetKeyDown(jumpKeyCode))
    //    {
    //        playerMove.Jump();
    //        playerMove.isLongJump = true;
    //    }
    //    else if (Input.GetKeyUp(jumpKeyCode))
    //    {
    //        playerMove.isLongJump = false;
    //    }

    //    int x = 0;
    //    if (Input.GetKey(leftKeyCode))
    //    {
    //        x -= 1;
    //    }
    //    if (Input.GetKey(rightKeyCode))
    //    {
    //        x += 1;
    //    }

    //    if (x != 0 && playerMove.IsGrounded)
    //    {
    //        OnWalking?.Invoke(x);
    //    }

    //    if (x != 0)
    //        playerMove.MoveToX(x);
    //    else if (!IsWireTensioned)
    //        playerMove.MoveToX(x);

    //    //playerMove.MoveToX(Input.GetAxis("Horizontal"));
    //}
    //#endregion

    //#region Wire
    //private void UpdateWire()
    //{
    //    if (isHookAnchored == true)
    //    {
    //        if (Input.GetKey(hookReturnKeyCode))
    //        {
    //            hookMove.ReturnHookShot();
    //        }
    //        if (Input.GetMouseButtonDown(1))
    //        {
    //            playerMove.WireJump();
    //        }
    //    }
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        hookMove.FireHookShot();
    //    }
    //}
    //#endregion
}
