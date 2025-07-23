using UnityEngine;

public class HookMove : BaseMove
{
    [SerializeField]
    private Rigidbody2D player;
    [SerializeField]
    private Transform home;

    [SerializeField]
    private LayerMask attachableLayer;
    [SerializeField]
    private Definition.FStat pullingForce = new Definition.FStat(10);
    public Definition.FStat PullingForce { get { return pullingForce; } }
    [SerializeField]
    private Definition.FStat maxWireLength = new Definition.FStat(10);
    public Definition.FStat MaxWireLength { get { return maxWireLength; } }
    [SerializeField]
    private float curWireLength;
    public float CurWireLength { get { return curWireLength; } set { curWireLength = value; } }

    private PlayerController playerController;
    override protected void Awake()
    {
        base.Awake();
        player.TryGetComponent(out playerController);
    }

    override protected void Update()
    {
        base.Update();
        UpdateHookShot();
    }

    #region HookShot
    [SerializeField]
    private bool isShooted = false;
    [SerializeField]
    private Vector2 fireDir;
    public void FireHookShot()
    {
        playerController.IsHookAnchored = false;
        isShooted = true;
        this.transform.position = home.position;
        curWireLength = maxWireLength.FinalStat();
        fireDir = playerController.MouseDir;
    }
    private void UpdateHookShot()
    {
        if (!isShooted)
        {
            this.transform.position = home.position;
            return;
        }

        if (playerController.IsHookAnchored)
        {
            return;
        }

        if (((Vector2)(player.transform.position - this.transform.position)).magnitude > maxWireLength.FinalStat())
        {
            ReturnHookShot();
            return;
        }
        MoveTo(fireDir);
    }
    public void AnchoringHookShot()
    {
        playerController.IsHookAnchored = true;
        rigid.linearVelocity = Vector2.zero;
        curWireLength = ((Vector2)(this.transform.position - player.transform.position)).magnitude;
    }
    public void ReturnHookShot()
    {
        playerController.IsHookAnchored = false;
        playerController.IsWireTensioned = false;
        isShooted = false;
        this.transform.position = home.position;
        rigid.linearVelocity = Vector2.zero;
        curWireLength = 0;
    }
    #endregion

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((attachableLayer.value & (1 << collision.gameObject.layer)) != 0)
        {
            AnchoringHookShot();
        }
    }

    //private void LateUpdate()
    //{
    //    WirePhysicsCal();
    //}
    //private void WirePhysicsCal()
    //{
    //    if (playerController.IsHookAnchored)
    //    {
    //        Vector2 dir = ((Vector2)(this.transform.position - player.transform.position)).normalized;
    //        float dis = ((Vector2)(this.transform.position - player.transform.position)).magnitude;

    //        if (dis >= curWireLength)
    //        {
    //            Vector2 playerSpeed = player.linearVelocity;
    //            if (Vector2.Dot(playerSpeed, dir) <= 0)
    //            {
    //                Vector2 newPlayerSpeed = playerSpeed - Vector2.Dot(playerSpeed, dir) * dir;
    //                player.linearVelocity = newPlayerSpeed;
    //            }
    //            player.transform.position = this.transform.position - (Vector3)(dir * curWireLength);
    //        }
    //    }
    //}


    //public void ZipUpWire()
    //{
    //    Vector2 dir = (this.transform.position - player.transform.position).normalized;
    //    float dis = (this.transform.position - player.transform.position).magnitude;

    //    if (dis < Time.deltaTime * pullingForce.FinalStat())
    //    {
    //        player.linearVelocity = Vector2.zero;
    //        player.transform.position = this.transform.position;
    //        return;
    //    }

    //    player.linearVelocity = dir * pullingForce.FinalStat();

    //    curWireLength = (this.transform.position - player.transform.position).magnitude;
    //}
}