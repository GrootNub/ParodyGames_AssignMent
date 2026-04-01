using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMotor : MonoBehaviour
{
    private CharacterController controller;

    public float speed = 6f;
    public float jumpForce = 8f;

    private Vector3 velocity;
    private Vector3 gravityDir;

    private bool isGrounded;

    float fallTimer = 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        gravityDir = GravityManager.Instance.CurrentGravity;

        GravityManager.Instance.OnGravityChanged += OnGravityChanged;
    }

    void Update()
    {
        ApplyGravity();
    }

    public void Move(Vector2 input)
    {
        Vector3 move = transform.right * input.x + transform.forward * input.y;
        controller.Move(move * speed * Time.deltaTime);
    }

    public void Jump()
    {
        if (isGrounded)
        {
            velocity = -gravityDir * jumpForce;
        }
    }

    /* void ApplyGravity()
     {
         isGrounded = CheckGround();

         if (isGrounded && velocity.magnitude < 1f)
         {
             velocity = Vector3.zero;
         }

         velocity += gravityDir * 20f * Time.deltaTime;
         controller.Move(velocity * Time.deltaTime);
     }*/

    void ApplyGravity()
    {
        isGrounded = CheckGround();

       
        if (!isGrounded)
        {
            fallTimer += Time.deltaTime;

            if (fallTimer > 2f)
            {
                GameManager.Instance.GameOver();
            }
        }
        else
        {
            fallTimer = 0f;
        }

       
        if (isGrounded && velocity.magnitude < 1f)
        {
            velocity = Vector3.zero;
        }

        velocity += gravityDir * 20f * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
    /* bool CheckGround()
     {
         return Physics.Raycast(transform.position, gravityDir, 1.2f);
     }*/

    bool CheckGround()
    {
        return Physics.Raycast(transform.position, gravityDir, 1.5f, LayerMask.GetMask("Ground"));
    }
    void OnGravityChanged(Vector3 newGravity)
    {
        gravityDir = newGravity;

       
        transform.up = -gravityDir;

        velocity = Vector3.zero;
    }
}