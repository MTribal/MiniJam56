using My_Utils;
using UnityEngine;

public class BasicPlatformerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private float moveDirection;

    private bool isGrounded;
    private float jumpTimer;
    private bool isJumping;
    public float moveSpeed;
    public float jumpForce;
    public float jumpMaxTime;
    public KeyCode jumpKey;
    public Transform feetPos;
    public Transform topHeadPos;
    public LayerMask whatIsGround;

    public bool editCheckGround;
    [ConditionalShow("editCheckGround", true)]
    public Vector2 checkGroundSize;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        moveDirection = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(moveDirection * moveSpeed, rb.velocity.y);
    }

    void Update()
    {
        isGrounded = Physics2D.OverlapBox(feetPos.position, checkGroundSize, 0, whatIsGround);

        if (Input.GetKey(jumpKey))
        {
            if (!isJumping && isGrounded)
            {
                rb.velocity = Vector2.up * jumpForce;
                isJumping = true;
                jumpTimer = jumpMaxTime;
            }

            if (isJumping)
            {
                if (jumpTimer > 0)
                {
                    rb.velocity = Vector2.up * jumpForce * MyLerp.Lerp(1, 0, 1 - jumpTimer / jumpMaxTime, EaseType.Exponential); // Makes the jump higher perfect smooth
                    jumpTimer -= Time.deltaTime;
                }
                else
                {
                    isJumping = false;
                }
            }
        }
        if (Input.GetKeyUp(jumpKey))
        {
            isJumping = false;
        }
    }

    private void OnDrawGizmos()
    {
        if (editCheckGround)
        {
            if (feetPos != null)
            {
                Gizmos.color = Color.white;
                Gizmos.DrawWireCube(feetPos.position, checkGroundSize);
            }
            else
            {
                Debug.LogWarning("Feet Pos is null.");
            }
        }
    }
}
