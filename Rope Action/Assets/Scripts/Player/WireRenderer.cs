using UnityEngine;

public class WireRenderer : MonoBehaviour
{
    private LineRenderer line;
    [SerializeField]
    private HookMove hookMove;

    [SerializeField]
    private Transform player;
    [SerializeField]
    private Transform hook;

    [SerializeField]
    private int pointCount;
    [SerializeField]
    private float slackScale;

    private void Awake()
    {
        line = this.GetComponent<LineRenderer>();
        line.positionCount = pointCount;
        line.useWorldSpace = true;
        //line.material.mainTextureScale = textureTileSize;
    }

    Vector2 endPos;
    Vector2 startPos;
    Vector2 center;
    float slack;
    Vector2 control;

    private float wireLength;
    private void LateUpdate()
    {
        if (hookMove == null) return;

        wireLength = hookMove.CurWireLength;

        if (player == null) return;
        if (hook == null) return;

        //UpdateLineTextureTiling(line);

        endPos = player.position;
        startPos = hook.position;
        center = (endPos + startPos) / 2;

        float dis = (startPos - endPos).magnitude;
        slack = dis - wireLength;

        control = center + Vector2.down * slack * slackScale;

        for (int i = 0; i < pointCount; i++)
        {
            line.SetPosition(i, CalculateCurve((float)i/pointCount));
        }
    }

    private Vector2 CalculateCurve(float t)
    {
        return Mathf.Pow(1 - t, 2) * startPos +
               2 * (1 - t) * t * control +
               Mathf.Pow(t, 2) * endPos;
    }



    //void UpdateLineTextureTiling(LineRenderer line)
    //{
    //    float length = GetLineLength(line);

    //    line.material.mainTextureScale = new Vector2(length * textureTileSize, textureTileSize);
    //}
    //float GetLineLength(LineRenderer lr)
    //{
    //    float length = 0f;
    //    for (int i = 0; i < lr.positionCount - 1; i++)
    //    {
    //        length += Vector3.Distance(lr.GetPosition(i), lr.GetPosition(i + 1));
    //    }
    //    return length;
    //}
}


