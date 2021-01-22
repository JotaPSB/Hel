using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField]
    private float maxHealth;

    [SerializeField]
    private GameObject
        deathChunkParticle,
        deathBloodParticle;

    private float currentHealth;
    private GameManager GM;

    public GameObject healthBar;
    private HealthBar healthBarS;
    private void Start()
    {
        currentHealth = maxHealth;
        healthBarS = healthBar.GetComponent<HealthBar>();
        healthBarS.SetMaxHealth(maxHealth);
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public void DecreaseHealth(float amount) 
    {
        currentHealth -= amount;
        healthBarS.SetHealth(currentHealth);
        if(currentHealth <= 0.0f)
        {
            Die();
        }
    }


    private void Die()
    {
        Instantiate(deathChunkParticle, transform.position, deathBloodParticle.transform.rotation);
        Instantiate(deathBloodParticle, transform.position, deathBloodParticle.transform.rotation);
    
        GM.Respawn();
        healthBarS.SetMaxHealth(maxHealth);
        Destroy(gameObject);
    }
}
