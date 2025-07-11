using UnityEngine;

public class HookController : BaseController
{
    [SerializeField]
    private bool isHookAnchored = false;
    public bool IsHookAnchored { get { return isHookAnchored; } }
}
