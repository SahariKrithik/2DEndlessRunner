using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float maxJumpTime = 0.3f;
    public float maxJumpHeight = 2f;
    public float baseFallMultiplier = 3f;
    public float baseFastFallMultiplier = 5f;
    public LayerMask groundLayer;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public float minYLimit = -10f;

    private Rigidbody rb;
    private Animator animator;
    private bool isGrounded;
    private bool isJumping;
    private float jumpTimeCounter;
    private float jumpStartY;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.drag = 0.15f;

        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (GameManager.Instance == null)
            return;

        bool isGameRunning = GameManager.Instance.isGameRunning;

        float gameSpeed = GameManager.Instance.gameSpeed;
        float speedScale = gameSpeed / GameManager.Instance.initialGameSpeed;

        float fallMultiplier = baseFallMultiplier * speedScale;
        float fastFallMultiplier = baseFastFallMultiplier * speedScale;

        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);

        // ✅ Update animation state and speed
        if (animator != null)
        {
            animator.SetBool("isRunning", isGameRunning);
            animator.speed = isGameRunning ? 0.6f * speedScale : 1f;
        }

        if (!isGameRunning)
            return;

        // Jump input
        bool jumpPressedDown = Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow);
        bool jumpHeld = Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
        bool jumpReleased = Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow);

        if (isGrounded && jumpPressedDown)
        {
            isJumping = true;
            jumpTimeCounter = maxJumpTime;
            jumpStartY = transform.position.y;

            float jumpVelocity = Mathf.Sqrt(-2f * Physics.gravity.y * maxJumpHeight);
            rb.velocity = new Vector3(rb.velocity.x, jumpVelocity, rb.velocity.z);
        }

        if (isJumping && jumpHeld)
        {
            float currentHeight = transform.position.y - jumpStartY;

            if (jumpTimeCounter > 0f && currentHeight < maxJumpHeight)
            {
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }

        if (jumpReleased)
        {
            isJumping = false;
        }

        // Apply falling gravity
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1f) * Time.deltaTime;
        }

        // Fast fall
        if (!isGrounded && (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)))
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (fastFallMultiplier - 1f) * Time.deltaTime;
        }

        // Reset on fall
        if (transform.position.y < minYLimit)
        {
            transform.position = new Vector3(0, 2, 0);
            rb.velocity = Vector3.zero;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Hit obstacle! Game Over!");
            GameManager.Instance.GameOver();
        }
    }
}
