


using System.Collections;
using UnityEngine;

// Ensures CharacterController is always attached
[RequireComponent(typeof(CharacterController))]
public class PlayerMotor : MonoBehaviour
{
    // Reference to Unity's CharacterController (handles movement & collisions)
    private CharacterController controller;

    // Movement speed and jump force
    public float speed = 6f;
    public float jumpForce = 8f;

    // Current velocity of the player (used for gravity & jumping)
    private Vector3 velocity;

    // Current gravity direction (changes dynamically)
    private Vector3 gravityDir;

    // Ground check flag
    private bool isGrounded;

    // Timer to detect free fall (for game over condition)
    float fallTimer = 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        // Initialize gravity from GravityManager
        gravityDir = GravityManager.Instance.CurrentGravity;

        // Subscribe to gravity change event
        GravityManager.Instance.OnGravityChanged += OnGravityChanged;
    }

    void Update()
    {
        // Apply gravity every frame
        ApplyGravity();
    }

    // Reference to camera for movement direction alignment
    public Transform cameraTransform;

    /// <summary>
    /// Handles player movement relative to camera orientation
    /// </summary>
    public void Move(Vector2 input)
    {
        if (cameraTransform == null) return;

        // Get camera forward & right directions
        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;

        // Remove vertical component so movement stays aligned with surface
        camForward = Vector3.ProjectOnPlane(camForward, transform.up).normalized;
        camRight = Vector3.ProjectOnPlane(camRight, transform.up).normalized;

        // Calculate movement direction
        Vector3 move = camRight * input.x + camForward * input.y;

        // Move player using CharacterController
        controller.Move(move * speed * Time.deltaTime);

        // Smoothly rotate player towards movement direction
        if (move != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(move, transform.up),
                Time.deltaTime * 10f
            );
        }
    }

    /// <summary>
    /// Handles jumping opposite to gravity direction
    /// </summary>
    public void Jump()
    {
        if (isGrounded)
        {
            // Jump in opposite direction of gravity
            velocity = -gravityDir * jumpForce;
        }
    }

    /// <summary>
    /// Applies gravity, handles grounding and fall detection
    /// </summary>
    void ApplyGravity()
    {
        // Check if player is touching surface
        isGrounded = CheckGround();

        //  FREE FALL DETECTION (Lose condition)
        if (!isGrounded)
        {
            fallTimer += Time.deltaTime;

            if (fallTimer > 2f)
            {
                GameManager.Instance.GameOver(false);
            }
        }
        else
        {
            fallTimer = 0f;
        }

        //  Surface sticking logic
        if (isGrounded && Vector3.Dot(velocity, gravityDir) >= 0)
        {
            // Keep player slightly pushed into surface to avoid floating
            velocity = gravityDir * 2f;
        }
        else
        {
            // Apply gravity acceleration
            velocity += gravityDir * 20f * Time.deltaTime;
        }

        // Apply vertical movement
        controller.Move(velocity * Time.deltaTime);
    }

    /// <summary>
    /// Checks if player is grounded using raycast in gravity direction
    /// </summary>
    bool CheckGround()
    {
        return Physics.Raycast(
            transform.position,
            gravityDir,
            1.5f,
            LayerMask.GetMask("Ground")
        );
    }

    /// <summary>
    /// Called when gravity direction changes
    /// Handles rotation, snapping, and stability
    /// </summary>
    void OnGravityChanged(Vector3 newGravity)
    {
        gravityDir = newGravity;

        //  Smoothly rotate player to new gravity direction
        StartCoroutine(SmoothRotate(newGravity));

        // Reset movement state
        velocity = Vector3.zero;
        fallTimer = 0f;

        //  Snap player to nearest surface in new gravity direction
        RaycastHit hit;

        if (Physics.Raycast(transform.position, gravityDir, out hit, 5f, LayerMask.GetMask("Ground")))
        {
            // Offset slightly to avoid clipping into surface
            transform.position = hit.point - gravityDir * 1.1f;
        }
    }

    /// <summary>
    /// Smooth rotation coroutine for gravity transitions
    /// </summary>
    IEnumerator SmoothRotate(Vector3 gravity)
    {
        Quaternion startRot = transform.rotation;

        // Calculate target rotation based on new gravity
        Quaternion targetRot =
            Quaternion.FromToRotation(transform.up, -gravity) * transform.rotation;

        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * 6f;

            transform.rotation = Quaternion.Slerp(startRot, targetRot, t);

            yield return null;
        }

        // Ensure exact final rotation
        transform.rotation = targetRot;
    }
}