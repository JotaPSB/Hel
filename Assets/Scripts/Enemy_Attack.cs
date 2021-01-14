using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Attack : MonoBehaviour
{
    #region Public Variables
    public Transform rayCast;
    public LayerMask raycastMask;
    public float rayCastLength;
    public float attackDistance;
    public float moveSpeed;
    public float timer;
    #endregion
    #region Private Variables
    private RaycastHit2D hit;
    private GameObject target;
    private Animator anim;
    private float distance;
    private bool attackMode;
    private bool inRange;
    private bool cooling;
    private float intTimer;
    #endregion

    void Awake()
    {
        intTimer = timer;
        anim = GetComponent<Animator>();

    }

    void Update()
    {
        if (inRange)
        {
            hit = Physics2D.Raycast(rayCast.position, Vector2.left, rayCastLength, raycastMask);
            RaycastDebugger();
        }

        if(hit.collider != null)
        {
            EnemyLogic();
        }else if(hit.collider == null)
        {
            inRange = false;
        }
        if(inRange == false)
        {
            anim.SetBool("canWalk", false);
            StopAttack();
        }
    }

   

    void EnemyLogic()
    {
        distance = Vector2.Distance(transform.position, target.transform.position);

        if (distance > attackDistance)
        {
            Move();
            StopAttack();
        }
        else if(attackDistance >= distance && cooling == false){
            Attack();
        }

        if (cooling)
        {
            Cooldown();
            anim.SetBool("attack", false);        }
    }

    void Move()
    {
        anim.SetBool("canWalk", true);
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("EnemyAttack"))
        {
            Vector2 targetPosition = new Vector2(target.transform.position.x, transform.position.y);

            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
    }

    private void Attack()
    {
        timer = intTimer; //Reset Timer when Player Attack Range
        attackMode = true; // To check if Enemy can still attack or not

        anim.SetBool("canWalk", false);
        anim.SetBool("attack", true);
    }


    void Cooldown()
    {
        timer -= Time.deltaTime;

        if(timer<= 0 && cooling && attackMode)
        {
            cooling = false;
            timer = intTimer;
        }
    }
    void StopAttack()
    {
        cooling = false;
        attackMode = false;
        anim.SetBool("attack", false);
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            target = collision.gameObject;
            inRange = true;
        }
    }
    void RaycastDebugger()
    {
        if(distance > attackDistance)
        {
            Debug.DrawRay(rayCast.position, Vector2.left * rayCastLength, Color.red);
        }
        else if(attackDistance > distance){
            Debug.DrawRay(rayCast.position, Vector2.left * rayCastLength, Color.green);
        }
    }
    public void TriggerCooling()
    {
        cooling = true;
    }
}
