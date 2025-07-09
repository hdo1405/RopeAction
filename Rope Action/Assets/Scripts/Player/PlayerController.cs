using UnityEngine;

public class PlayerController : BaseController
{
    [SerializeField]
    private KeyCode jumpKeyCode = KeyCode.Space;
    [SerializeField]
    private KeyCode leftKeyCode = KeyCode.A;
    [SerializeField]
    private KeyCode rightKeyCode = KeyCode.D;

    private void Update()
    {
        UpdateMove();
    }

    private void UpdateMove()
    {
        if (Input.GetKeyDown(jumpKeyCode))
        {
            baseMove.Jump();
            baseMove.isLongJump = true;
        }
        else if (Input.GetKeyUp(jumpKeyCode))
        {
            baseMove.isLongJump = false;
        }

        float x = 0;
        if (Input.GetKey(leftKeyCode))
        {
            x -= 1;
        }
        if (Input.GetKey(rightKeyCode))
        {
            x += 1;
        }

        baseMove.MoveToX(x);

        //baseMove.MoveToX(Input.GetAxis("Horizontal"));
    }
}
