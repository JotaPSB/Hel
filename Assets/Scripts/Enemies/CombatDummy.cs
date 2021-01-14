using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatDummy : MonoBehaviour
{
    [SerializeField]
    private float maxHealth, knockbackSpeedX, knockbackSpeedY, knockbackDuration, knockbackDeathSpeedX, knockbackDeathSpeedY, deathTorque;
    [SerializeField]
    private bool applyKnockback;
    [SerializeField]
    private GameObject hitParticle;

    private bool playerOnLeft, knockback;

    private int playerFacingDirection;

    private AttackDetails attackDetails;
    private float currentHealth, knockbackStart;

    private PlayerMovement pc;
    private GameObject aliveGO, brokenTopGO, brokenBotGO;
    private Rigidbody2D rbAlive, rbBrokentTop, rbBrokenBot;
    private Animator aliveAnim;

    private void Start()
    {
        currentHealth = maxHealth;

        pc = GameObject.Find("Player").GetComponent<PlayerMovement>();

        aliveGO = transform.Find("Alive").gameObject;
        brokenTopGO = transform.Find("Broken Top").gameObject;
        brokenBotGO = transform.Find("Broken Bottom").gameObject;

        aliveAnim = aliveGO.GetComponent<Animator>();
        rbAlive = aliveGO.GetComponent<Rigidbody2D>();
        rbBrokentTop = brokenTopGO.GetComponent<Rigidbody2D>();
        rbBrokenBot = brokenBotGO.GetComponent<Rigidbody2D>();


        aliveGO.SetActive(true);
        brokenBotGO.SetActive(false);
        brokenTopGO.SetActive(false);

    }

    private void Update()
    {
        CheckKnockback();
    }

    private void Damage(AttackDetails attackDetails)
    {
        currentHealth -= attackDetails.damageAmount;
        
      
        Instantiate(hitParticle, aliveGO.transform.position, Quaternion.Euler(0.0f, 0.0f, Random.Range(0.0f, 360.0f)));
        if (attackDetails.position.x < aliveGO.transform.position.x)
        {
            playerFacingDirection = 1;

        }
        else
        {
            playerFacingDirection = -1;
        }

        aliveAnim.SetBool("PlayerOnLeft", playerOnLeft);
        aliveAnim.SetTrigger("damage");

        if(applyKnockback && currentHealth > 0.0f)
        {
            //Knockback
            Knockback();
        }

        if(currentHealth <= 0.0f)
        {
            Die();
        }
    }
    private void Knockback()
    {
        knockback = true;
        knockbackStart = Time.time;
        rbAlive.velocity = new Vector2(knockbackSpeedX * playerFacingDirection, knockbackSpeedY);
    }

    private void CheckKnockback()
    {
        if (Time.time> knockbackStart + knockbackDuration && knockback)
        {
            knockback = false;
            rbAlive.velocity = new Vector2(0.0f, rbAlive.velocity.y);
        }
    }

    private void Die()
    {
        aliveGO.SetActive(false);
        brokenTopGO.SetActive(true);
        brokenBotGO.SetActive(true);

        brokenTopGO.transform.position = aliveGO.transform.position;
        brokenBotGO.transform.position = aliveGO.transform.position;

        rbBrokenBot.velocity = new Vector2(knockbackSpeedX * playerFacingDirection, knockbackSpeedY);
        rbBrokentTop.velocity = new Vector2(knockbackDeathSpeedX * playerFacingDirection, knockbackDeathSpeedY);
        rbBrokentTop.AddTorque(deathTorque * -playerFacingDirection, ForceMode2D.Impulse);
    }
}
