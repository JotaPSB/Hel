using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArgo : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    Transform player;
    [SerializeField]
    float agroRange;
    [SerializeField]
    float moveSpeed;

    Rigidbody2D rb2d;
    Animator animator;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //distance to player
        float distToPlayer = Vector2.Distance(transform.position, player.position);
        if (distToPlayer < agroRange)
        {
            animator.SetBool("walk", true);
            //code to chase player
            ChasePlayer();
        }
        else
        {
            animator.SetBool("walk", false);
            //stop chasing player
            StopChasingPlayer();
        }
    }


    void ChasePlayer()

    {
        if(transform.position.x < player.position.x)
        {
            //enemy is to the left side of the player, so move right
            rb2d.velocity = new Vector2(moveSpeed, rb2d.velocity.y);
            transform.localScale = new Vector2(1, 1);

        }
        else
        {
            //enemy is to the right side of the player, so move left
            rb2d.velocity = new Vector2(-moveSpeed, 0);
            transform.localScale = new Vector2(-1, 1);
        }
    }
    void StopChasingPlayer()
    {
        rb2d.velocity = new Vector2(0, 0);
    }
}
