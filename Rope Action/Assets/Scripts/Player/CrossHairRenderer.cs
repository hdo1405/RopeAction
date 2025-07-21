using UnityEngine;

public class CrossHairRenderer : MonoBehaviour
{
    private LineRenderer line;
    [SerializeField]
    private HookMove hookMove;
    [SerializeField]
    private PlayerController playerController;
    [SerializeField]
    private Transform player;

    [SerializeField]
    private bool isEnabled = false;

    private void Awake()
    {
        line = this.GetComponent<LineRenderer>();
        line.positionCount = 2;
        line.useWorldSpace = true;
        //line.material.mainTextureScale = textureTileSize;
    }

    void Update()
    {
        if (isEnabled)
        {
            line.enabled = true;
            line.SetPosition(0, player.position);
            line.SetPosition(1, (Vector2)player.position + (playerController.MouseDir * hookMove.MaxWireLength.FinalStat()));
        }
        else
        {
            line.enabled = false;
        }
    }
}
