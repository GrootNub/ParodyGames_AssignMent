using UnityEngine;

public class InputManager : MonoBehaviour
{
    public Vector2 MoveInput { get; private set; }
    public bool JumpPressed { get; private set; }
    public Vector3 GravityInput { get; private set; }
    public bool ConfirmGravity { get; private set; }

    void Update()
    {
        MoveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        JumpPressed = Input.GetKeyDown(KeyCode.Space);

        ConfirmGravity = Input.GetKeyDown(KeyCode.Return);

        GravityInput = Vector3.zero;

        if (Input.GetKey(KeyCode.UpArrow)) GravityInput = Vector3.forward;
        if (Input.GetKey(KeyCode.DownArrow)) GravityInput = Vector3.back;
        if (Input.GetKey(KeyCode.LeftArrow)) GravityInput = Vector3.left;
        if (Input.GetKey(KeyCode.RightArrow)) GravityInput = Vector3.right;
    }
}