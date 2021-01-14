using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private float movementInputDirecion=0;
    private float knockbackStartTime;
    [SerializeField]
    private float knockbackDuration;

    private int amountOfJumpsLeft;

    private bool isFacingRight = true;
    private bool isWalking;
    private bool isTouchingWall;
    private bool isWallSliding;
    private bool isGrounded;
    private bool canJump;
    private bool knockback;

    public float movementSpeed = 2;
    public float jumpForce = 4;
    public float groundCheckRadius;
    public float wallCheckDistance;
    public float wallSlideSpeed;
    public float movementForceInAir;

    [SerializeField]
    private Vector2 knockbackSpeed;

    private Rigidbody2D rb;
    private Animator anim;

    public int amountOfJumps = 1;

    public LayerMask whatIsGround;
    public Transform groundCheck;
    public Transform wallCheck;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        amountOfJumpsLeft = amountOfJumps;
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
        CheckMovementDireciont();
        UpdateAnimation();
        CheckIfCanJump();
        CheckIfWallSliding();
        CheckKnockback();
    }

    private void FixedUpdate()
    {
        ApplyMovement();
        CheckSurroundings();
    }

    private void CheckIfWallSliding()
    {
        if(isTouchingWall && !isGrounded && rb.velocity.y<0)
        {
            isWallSliding = true;
        }
        else
        {
            isWallSliding = false;
        }
    }

    public void Knockback(int direction)
    {
        knockback = true;
        knockbackStartTime = Time.time;
        rb.velocity = new Vector2(knockbackSpeed.x * direction, knockbackSpeed.y);
    }

    private void CheckKnockback()
    {
        if(Time.time >= knockbackStartTime + knockbackDuration && knockback)
        {
            knockback = false;
            rb.velocity = new Vector2(0.0f, rb.velocity.y);
        }
    }

    private void CheckSurroundings()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);

        isTouchingWall = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance, whatIsGround);
    }

    private void CheckIfCanJump()
    {
        if(isGrounded && rb.velocity.y <= 0)
        {
            amountOfJumpsLeft = amountOfJumps;
        }

        if (amountOfJumpsLeft <= 0)
        {
            canJump = false;
        }
        else
        {
            canJump = true;
        }
    }

    private void CheckMovementDireciont()
    {
        if (isFacingRight && movementInputDirecion < 0)
        {
            Flip();
        }
        else if (!isFacingRight && movementInputDirecion > 0)
        {
            Flip();
        }

        if(rb.velocity.x!= 0)
        {
            isWalking = true;
        }
        else
        {
            isWalking = false;
        }
    }

    private void UpdateAnimation()
    {
        anim.SetBool("isWalking", isWalking);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("yVelocity", rb.velocity.y);
    }

    private void CheckInput()
    {
        movementInputDirecion = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }
    }

    private void ApplyMovement()
    {

        if (isGrounded && !knockback)
        {
            rb.velocity = new Vector2(movementSpeed * movementInputDirecion, rb.velocity.y);
        }
        else if(!isGrounded && !isWallSliding && movementInputDirecion != 0 && !knockback)
        {
            Vector2 forceToAdd = new Vector2(movementForceInAir * movementInputDirecion, 0);
            rb.AddForce(forceToAdd);

            if(Mathf.Abs(rb.velocity.x) > movementSpeed)
            {
                rb.velocity = new Vector2(movementSpeed * movementInputDirecion, rb.velocity.y);
            }
        }


        if (isWallSliding)
        {
            if(rb.velocity.y< -wallSlideSpeed)
            {
                rb.velocity = new Vector2(rb.velocity.x, -wallSlideSpeed);
            }
        }
    }

    private void Jump()
    {
        if (canJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            amountOfJumpsLeft--;
        }
    }

    private void Flip()
    {
        if(!isWallSliding && !knockback)
        {
            isFacingRight = !isFacingRight;
            transform.Rotate(0.0f, 180.0f, 0.0f);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);

        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x+wallCheckDistance, wallCheck.position.y, wallCheck.position.z));
    }
}
