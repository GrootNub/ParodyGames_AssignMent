using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerMotor motor;
    private InputManager input;
    [SerializeField] private GameObject hologramPlayer;

    Animator animator;

    void Start()
    {
        motor = GetComponent<PlayerMotor>();
        input = FindObjectOfType<InputManager>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {

        float speed = input.MoveInput.magnitude;
        animator.SetFloat("Speed", speed);

        motor.Move(input.MoveInput);

        if (input.JumpPressed)
            motor.Jump();

        HandleGravityInput();
    }

    /*void HandleGravityInput()
    {
        if (input.GravityInput != Vector3.zero)
        {
            // Preview handled later
        }

        if (input.ConfirmGravity && input.GravityInput != Vector3.zero)
        {
            GravityManager.Instance.SetGravity(input.GravityInput);
        }
    }*/

    void HandleGravityInput()
    {
        if (input.GravityInput != Vector3.zero)
        {
           
            hologramPlayer.SetActive(true);

           
            hologramPlayer.transform.position =
                transform.position + input.GravityInput * 2f;

           
            hologramPlayer.transform.up = -input.GravityInput;
        }
        else
        {
            
            hologramPlayer.SetActive(false);
        }

        if (input.ConfirmGravity && input.GravityInput != Vector3.zero)
        {
            GravityManager.Instance.SetGravity(input.GravityInput);

           
            hologramPlayer.SetActive(false);
        }
    }
}