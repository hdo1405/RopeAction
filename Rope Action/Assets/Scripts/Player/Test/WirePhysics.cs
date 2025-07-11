using UnityEngine;

public class WirePhysics : BaseMove
{
    [SerializeField]
    private Rigidbody2D player;
    [SerializeField]
    private Transform home;

    [SerializeField]
    private LayerMask attachableLayer;
    [SerializeField]
    private Definition.FStat pullingForce = new Definition.FStat(10);
    [SerializeField]
    private Definition.FStat maxWireLength = new Definition.FStat(10);
    [SerializeField]
    private float curWireLength;
    public float CurWireLength { get { return curWireLength; } }

    private PlayerController playerController;
    override protected void Awake()
    {
        base.Awake();
        mainCamera = Camera.main;
        player.TryGetComponent<PlayerController>(out playerController);
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
    private Camera mainCamera;
    public void FireHookShot()
    {
        playerController.IsHookAnchored = false;
        isShooted = true;
        this.transform.position = home.position;
        curWireLength = maxWireLength.FinalStat();
        fireDir = (mainCamera.ScreenToWorldPoint(Input.mousePosition) - this.transform.position).normalized;
    }
    private void UpdateHookShot()
    {
        Debug.Log(isShooted);

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

    protected void LateUpdate()
    {
        WirePhysicsCal();
    }
    private void WirePhysicsCal()
    {
        if (playerController.IsHookAnchored)
        {
            Vector2 dir = ((Vector2)(player.transform.position - this.transform.position)).normalized;
            float dis = ((Vector2)(player.transform.position - this.transform.position)).magnitude;

            if (dis >= curWireLength)
            {
                playerController.IsWireTensioned = true;

                Vector2 playerSpeed = player.linearVelocity;
                if (Vector2.Dot(playerSpeed, dir) >= 0)
                {
                    Vector2 newPlayerSpeed = playerSpeed - Vector2.Dot(playerSpeed, dir) * dir;
                    player.linearVelocity = newPlayerSpeed;
                }
                player.transform.position = this.transform.position + (Vector3)(dir * curWireLength);
            }
            else
            {
                playerController.IsWireTensioned = false;
            }
        }
    }


    public void ZipUpWire()
    {
        curWireLength -= Time.deltaTime * pullingForce.FinalStat();

        if (curWireLength <= 0)
        {
            curWireLength = 0;
            player.linearVelocity = Vector2.zero;
            player.transform.position = this.transform.position;
        }
        //Vector2 dir = (this.transform.position - player.transform.position).normalized;
        //float dis = (this.transform.position - player.transform.position).magnitude;

        //if (dis < Time.deltaTime * pullingForce.FinalStat())
        //{
        //    player.linearVelocity = Vector2.zero;
        //    player.transform.position = this.transform.position;
        //    return;
        //}

        //player.linearVelocity = dir * pullingForce.FinalStat();

        //curWireLength = (this.transform.position - player.transform.position).magnitude;
    }
}