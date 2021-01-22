using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private float movementInputDirecion=0;
    private float knockbackStartTime;
    [SerializeField]
    private float knockbackDuration;

    private float dashTimeLeft;
    private float lastImageXpos;
    private float lastDash = -100f;

    private int amountOfJumpsLeft;
    private int facingDirection = 1;

    private bool isFacingRight = true;
    private bool isWalking;
    private bool isTouchingWall;
    private bool isWallSliding;
    private bool isGrounded;
    private bool canJump;
    private bool knockback;
    private bool isDashing;

    public float movementSpeed = 2;
    public float jumpForce = 4;
    public float groundCheckRadius;
    public float wallCheckDistance;
    public float wallSlideSpeed;
    public float movementForceInAir;
    public float wallHopForce;
    public float wallJumpForce;
    public float dashTime;
    public float dashSpeed;
    public float distanceBetweenImages;
    public float dashCooldown;

    public Vector2 wallHopDirection;
    public Vector2 wallJumpDirection;

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
        wallHopDirection.Normalize();
        wallJumpDirection.Normalize();
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
        CheckDash();
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
        if((isGrounded && rb.velocity.y <= 0) || isWallSliding)
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
        anim.SetBool("isWallSliding", isWallSliding);
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

        if (Input.GetButtonDown("Dash"))
        {
            if(Time.time >= lastDash + dashCooldown)
            {
                AttemptToDash();

            }
        }
    }

    private void AttemptToDash()
    {
        isDashing = true;
        dashTimeLeft = dashTime;
        lastDash = Time.time;

        PlayerAfterImagePool.Instance.GetFromPool();
        lastImageXpos = transform.position.x;
    }

    private void CheckDash()
    {
        if (isDashing)
        {
            if (dashTimeLeft > 0)
            {
                rb.velocity = new Vector2(dashSpeed * facingDirection, rb.velocity.y);
                dashTimeLeft -= Time.deltaTime;
                if (Mathf.Abs(transform.position.x - lastImageXpos) > distanceBetweenImages)
                {
                    PlayerAfterImagePool.Instance.GetFromPool();
                    lastImageXpos = transform.position.x;
                }

                if (dashTimeLeft <= 0 || isTouchingWall)
                {
                    isDashing = false;
                }
            }
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
        if (canJump && !isWallSliding)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            amountOfJumpsLeft--;
        }else if (isWallSliding && movementInputDirecion == 0 && canJump)
        {
            isWallSliding = false;
            amountOfJumpsLeft--;
            Vector2 forceToAdd = new Vector2(wallHopForce * wallHopDirection.x * -facingDirection, wallHopForce*wallHopDirection.y);
            rb.AddForce(forceToAdd, ForceMode2D.Impulse);
        }
        else if((isWallSliding || isTouchingWall)&& movementInputDirecion != 0 && canJump)
        {
            isWallSliding = false;
            amountOfJumpsLeft--;
            Vector2 forceToAdd = new Vector2(wallJumpForce * wallJumpDirection.x * movementInputDirecion, wallJumpForce * wallJumpDirection.y);
            rb.AddForce(forceToAdd, ForceMode2D.Impulse);
        }
    }

    private void Flip()
    {
        if(!isWallSliding && !knockback)
        {
            facingDirection *= -1;
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
