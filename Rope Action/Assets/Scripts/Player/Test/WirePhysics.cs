using UnityEngine;

public class WirePhysics : MonoBehaviour
{
    public Rigidbody2D player;
    public float pullingForce;

    public float curRopeLength = 10f;

    void Update()
    {
        Vector2 dir = (this.transform.position - player.transform.position).normalized;
        float dis = (this.transform.position - player.transform.position).magnitude;

        if (Input.GetKey(KeyCode.Space))
        {
            if (dis < Time.deltaTime * pullingForce)
            {
                player.linearVelocity = Vector2.zero;
                player.transform.position = this.transform.position;
                return;
            }

            player.linearVelocity = dir * pullingForce;

            curRopeLength = (this.transform.position - player.transform.position).magnitude;
        }
        else if (Input.GetMouseButton(1))
        {
            curRopeLength = 10f;
        }


    }

    private void LateUpdate()
    {
        Vector2 dir = ((Vector2)(this.transform.position - player.transform.position)).normalized;
        float dis = ((Vector2)(this.transform.position - player.transform.position)).magnitude;

        Debug.Log(dis);

        if (dis >= curRopeLength)
        {
            Vector2 playerSpeed = player.linearVelocity;
            if (Vector2.Dot(playerSpeed, dir) <= 0)
            {
                Vector2 newPlayerSpeed = playerSpeed - Vector2.Dot(playerSpeed, dir) * dir;
                player.linearVelocity = newPlayerSpeed;
            }
            player.transform.position = this.transform.position - (Vector3)(dir * curRopeLength);
        }
    }
}
