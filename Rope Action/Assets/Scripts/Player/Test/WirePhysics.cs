using UnityEngine;

public class WirePhysics : MonoBehaviour
{
    public Rigidbody2D player;
    public float pullingForce;

    public float curRopeLength = 10f;

    void Update()
    {
        Vector2 dir = (this.transform.position - player.transform.position).normalized;
        float dis = (this.transform.position - player.transform.position).sqrMagnitude;

        if (Input.GetKey(KeyCode.Space))
        {
            if (dis < Time.deltaTime * pullingForce)
            {
                player.linearVelocity = Vector2.zero;
                player.transform.position = this.transform.position;
                return;
            }

            Debug.Log(dir);
            player.linearVelocity = dir * pullingForce;

            curRopeLength = (this.transform.position - player.transform.position).sqrMagnitude;
        }
        else if (Input.GetMouseButton(1))
        {
            curRopeLength = 10f;
        }


        if (dis > curRopeLength)
        {
            Vector2 playerSpeed = player.linearVelocity;
            Vector2 newPlayerSpeed = playerSpeed - Vector2.Dot(playerSpeed, dir) * dir;

            player.linearVelocity = newPlayerSpeed;
        }
    }
}
