using UnityEngine;

public class WirePhysicsMove : BaseMove
{
    [SerializeField] float pullingTime;

    private bool isPulled = false;

    public void Pulling(float? pullingTime = null)
    {
        isPulled = true;

        if (pullingTime == null)
            Invoke(nameof(DisableIsPulled), this.pullingTime);
        else
            Invoke(nameof(DisableIsPulled), (float)pullingTime);
    }

    public void DisableIsPulled()
    {
        isPulled = false;
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (isPulled)
        {
            if (Physics2D.OverlapCircle(this.transform.position, 3f, 1 << LayerMask.NameToLayer("Player")) != null)
            {
                rigid.linearVelocity *= 0.4f;
                Debug.Log("느려짐");
            }
        }
    }

    //[Header("---------------------------------------------------------------------------------------")]
    //[Header("와이어 관련")]

    //[SerializeField]
    //private bool isAnchoredByHook = false;

    //[SerializeField]
    //private Definition.FStat mass = new Definition.FStat(1);
    //public Definition.FStat Mass { get { return mass; } }

    //private HookMove hookMove;
    //private Rigidbody2D hookRigid;
    //private Rigidbody2D playerRigid;
    //private PlayerController playerController;

    //public void AnchoringHook(HookMove _hook, PlayerController _playerController)
    //{
    //    hookMove = _hook;
    //    hookRigid = hookMove.GetComponent<Rigidbody2D>();
    //    playerController = _playerController;
    //    playerRigid = playerController.GetComponent<Rigidbody2D>();

    //    isAnchoredByHook = true;
    //}

    //protected override void Update()
    //{
    //    base.Update();
    //    CheckHookAnchored();
    //    WirePhysicsCal();
    //}
    //private void WirePhysicsCal()
    //{
    //    if (!isAnchoredByHook) return;

    //    Vector2 dir = ((Vector2)(playerRigid.transform.position - this.transform.position)).normalized;
    //    float dis = ((Vector2)(playerRigid.transform.position - this.transform.position)).magnitude;

    //    if (dis >= hookMove.CurWireLength)
    //    {
    //        playerController.IsWireTensioned = true;

    //        Vector2 curSpeed = rigid.linearVelocity;
    //        if (Vector2.Dot(curSpeed, dir) <= 0)
    //        {
    //            Vector2 newCurSpeed = curSpeed - Vector2.Dot(curSpeed, dir) * dir;
    //            rigid.linearVelocity = newCurSpeed;
    //        }
    //        rigid.transform.position = playerRigid.transform.position - (Vector3)(dir * hookMove.CurWireLength);
    //    }
    //    else
    //    {
    //        playerController.IsWireTensioned = false;
    //    }

    //}

    //public void CheckHookAnchored()
    //{
    //    if (hookMove == null) return;

    //    if (hookMove.IsHookAnchoredAtOBJ == false)
    //    {
    //        hookMove = null;
    //        isAnchoredByHook = false;
    //    }
    //}
}
