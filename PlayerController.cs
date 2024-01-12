using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public float jumpForce;
    public int jumpAmount;

    private Rigidbody2D myRigidbody;

    private bool isGrounded;
    public Transform feetPos;
    public float checkRadius;
    public LayerMask whatIsGround;

    private float jumpTimeCounter;
    public float jumpTime;
    private bool isJumping;

    private float storedGravity;
    private bool canMove = true;
    private float moveInput;

    public float jumpBufferLength = 0.2f; // Time in seconds for how long the jump input is remembered
    private float jumpBufferCount; // Counter for the buffer time

    void Start()
    {
        // Get the Rigidbody2D component from the player
        myRigidbody = GetComponent<Rigidbody2D>();
        storedGravity = myRigidbody.gravityScale;
        //jumps = jumpAmount;
    }
    
    void Update()
    {
        if (canMove)
        {
            isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGround);

            if ((Input.GetKeyDown(KeyCode.Space) || jumpBufferCount > 0) && isGrounded)
            {
                isJumping = true;
                jumpTimeCounter = jumpTime;
                myRigidbody.velocity = Vector2.up * jumpForce;
                jumpBufferCount = 0;
            }

            if (Input.GetKey(KeyCode.Space) && isJumping == true)
            {
                if (jumpTimeCounter > 0)
                {
                    myRigidbody.velocity = Vector2.up * jumpForce;
                    jumpTimeCounter -= Time.deltaTime;
                }
                else
                {
                    isJumping = false;
                }
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                jumpBufferCount = jumpBufferLength;
            }
            else
            {
                jumpBufferCount -= Time.deltaTime;
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                isJumping = false;
            }
        }
    }


    void FixedUpdate()
    {
        if (canMove == true)
        {
            float targetSpeed = Input.GetAxisRaw("Horizontal") * moveSpeed;
            float accelerationRate = 40;
            // Interpolate the velocity towards the target speed
            myRigidbody.velocity = new Vector2(
                Mathf.Lerp(myRigidbody.velocity.x, targetSpeed, Time.fixedDeltaTime * accelerationRate),
                myRigidbody.velocity.y
            );
        }
    }

    // Check if the player is touching the ground
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground")
        {
            MovementOn();
            myRigidbody.gravityScale = storedGravity;
            //jumps = jumpAmount;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Sling"))
        {
            myRigidbody.transform.position = other.transform.position;
            myRigidbody.velocity = Vector3.zero;
            myRigidbody.gravityScale = 0;
        }
    }

    // Check if the player has stopped touching the ground
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground")
        {
        }
    }

    public void MovementOff()
    {
        canMove = false;
    }

    public void MovementOn()
    {
        canMove = true;
    }

    public float GetStoredGravity()
    {
        return storedGravity;
    }
}
