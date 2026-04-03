



using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Reference to movement + physics handler
    private PlayerMotor motor;

    // Handles all player input (WASD, jump, gravity)
    private InputManager input;

    // Hologram preview object (shows gravity direction)
    [SerializeField] private GameObject hologramPlayer;

    // Separate camera target to avoid camera breaking during gravity changes
    [SerializeField] private Transform cameraTarget;

    // Animator for controlling player animations
    Animator animator;

    void Start()
    {
        // Get required components
        motor = GetComponent<PlayerMotor>();
        input = FindObjectOfType<InputManager>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Keep camera target aligned with player position and orientation
        // This ensures Cinemachine behaves correctly even when gravity changes
        cameraTarget.position = transform.position + transform.up * 1.5f;
        cameraTarget.rotation = Quaternion.LookRotation(transform.forward, transform.up);

        //  Update animation based on movement input
        float speed = input.MoveInput.magnitude;
        animator.SetFloat("Speed", speed);

        //  Handle player movement
        motor.Move(input.MoveInput);

        // 🪂 Handle jump
        if (input.JumpPressed)
            motor.Jump();

        // Handle gravity preview + switching
        HandleGravityInput();
    }

    /// <summary>
    /// Handles gravity direction preview (hologram)
    /// and applies gravity when confirmed
    /// </summary>
    void HandleGravityInput()
    {
        Vector3 worldDir = Vector3.zero;

        //  Convert input (arrow keys) into world direction relative to player
        if (input.GravityInput != Vector3.zero)
        {
            worldDir =
                transform.forward * input.GravityInput.z +
                transform.right * input.GravityInput.x;
        }

        // SHOW HOLOGRAM PREVIEW
        if (worldDir != Vector3.zero)
        {
            hologramPlayer.SetActive(true);

            worldDir.Normalize();

            //  Position hologram slightly in front of player
            hologramPlayer.transform.position =
                transform.position + worldDir * 1f;

            //  Rotate hologram to face direction (keeping it upright)
            Vector3 flatDir = Vector3.ProjectOnPlane(worldDir, Vector3.up);

            if (flatDir != Vector3.zero)
            {
                hologramPlayer.transform.rotation =
                    Quaternion.LookRotation(flatDir);
            }
        }
        else
        {
            // Hide hologram when no gravity input
            hologramPlayer.SetActive(false);
        }

        //  APPLY GRAVITY CHANGE (on Enter key)
        if (input.ConfirmGravity && input.GravityInput != Vector3.zero)
        {
            Vector3 gravityDir =
                transform.forward * input.GravityInput.z +
                transform.right * input.GravityInput.x;

            //  Update global gravity direction
            GravityManager.Instance.SetGravity(gravityDir);

            // Hide preview after applying
            hologramPlayer.SetActive(false);
        }
    }
}