using UnityEngine;

public class WireRenderer : MonoBehaviour
{
    private LineRenderer line;

    [SerializeField]
    private Transform player;
    [SerializeField]
    private Transform hook;
    //[SerializeField]
    //Vector2D textureTileSize;

    private void Awake()
    {
        line = this.GetComponent<LineRenderer>();
        line.positionCount = 2;
        line.useWorldSpace = true;
        //line.material.mainTextureScale = textureTileSize;
    }

    private void LateUpdate()
    {
        if (player == null) return;
        if (hook == null) return;

        //UpdateLineTextureTiling(line);

        Vector3 endPos = player.position;
        Vector3 startPos = hook.position;

        startPos.z += 1;
        endPos.z += 1;

        line.SetPosition(0, startPos);
        line.SetPosition(1, endPos);
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


