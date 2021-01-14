using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerJump : MonoBehaviour
{
    //fuerza, aplicar fuerza, 1x
    //rb.velocity = new Vector2(rb.velocity.x, jumpForce);

    [Header("Jump Details")]
    public float jumpForce;
    public float jumpTime;
    private float jumpTimeCounter;
    private bool stoppedJumping;

    [Header("Ground Details")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private float radCircle;
    public bool grounded;

    [Header("Components")]
    private Rigidbody2D rb;
    private Animator myAnimator;

    //myAnimator.SetBool("falling", true);
    //myAnimator.SetBool("falling", false);

    //myAnimator.SetTrigger("jump");
    //myAnimator.ResetTrigger("jump");

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        jumpTimeCounter = jumpTime;
    }
    
    private void Update()
    {
        //lo que significa estar en el suelo
        grounded = Physics2D.OverlapCircle(groundCheck.position, radCircle, whatIsGround);
        if (grounded)
        {
            jumpTimeCounter = jumpTime;
            myAnimator.ResetTrigger("jump");
            myAnimator.SetBool("falling", false);
        }

        // si usamos el espacio y la w para saltar
        if (Input.GetButtonDown("Jump") && grounded)
        {
            //salta!!
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            stoppedJumping = false;
            //dice al animador que reproduzca la animacion
            myAnimator.SetTrigger("jump");
        }
        //para seguir saltando mientras el boton este activo
        if ((Input.GetButton("Jump") && !stoppedJumping) && (jumpTimeCounter >0))
        {
            //jump!!!
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);

            jumpTimeCounter -= Time.deltaTime;
            myAnimator.SetTrigger("jump");
        }

        //si soltamos boton de saltar
        if (Input.GetButtonUp("Jump"))
        {
            jumpTimeCounter = 0;
            stoppedJumping = true;
            myAnimator.SetBool("falling", true);
            myAnimator.ResetTrigger("jump");
        }
        if (rb.velocity.y < 0)
        {
            myAnimator.SetBool("falling", true);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(groundCheck.position, radCircle);
    }
    private void FixedUpdate()
    {
        HandleLayers();
    }
    private void HandleLayers()
    {
        if (!grounded)
        {
            myAnimator.SetLayerWeight(1, 1);
        }
        else
        {
            myAnimator.SetLayerWeight(1, 0);
        }
    }
}
