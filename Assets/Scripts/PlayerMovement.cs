using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

[RequireComponent (typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    //necesarios para animaciones y fisicas
    private Rigidbody2D rb2d;
    private Animator myAnimator;

    //variables para usar
    public float speed = 2.0f;
    public float horizMovement;
    private bool facingRight = true;

    // Start is called before the first frame update
    private void Start()
    {
        //definir los gameobjects en el jugador
        rb2d = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
    }

    // maneja el input para las fisicas
    private void Update()
    {
        //mira en que direccion se esta moviendo
        horizMovement = Input.GetAxisRaw("Horizontal");
    }
    //maneja las fisicas de correr
    private void FixedUpdate()
    {
        //mueve el personaje de izquierda a derecha
        rb2d.velocity = new Vector2(horizMovement * speed, rb2d.velocity.y);
        Flip(horizMovement);
        myAnimator.SetFloat("speed", Mathf.Abs(horizMovement));
    }
    //funcion de girar

    private void Flip(float hm)
    {
        if (hm <0 && facingRight || hm > 0 && !facingRight)
        {
            facingRight = !facingRight;
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
    }

    public bool GetFacingDirection()
    {
        return facingRight;

    }

}
