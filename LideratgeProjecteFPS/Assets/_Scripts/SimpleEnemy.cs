using System;
using UnityEngine;

class SimpleEnemy : MonoBehaviour, IShootable
{
    [SerializeField] float startHealth;
    private float currentHealth;

    [SerializeField] float damage;
    [SerializeField] float meleeAttackDistance;
    [SerializeField] float attackRate;
    private float attackTimer;
    GameObject player;


    void Start()
    {
        currentHealth = startHealth;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(gameObject.transform.position, meleeAttackDistance);
    }

    void Update()
    {
        if (PlayerOnRange())
            Attack();
        else if (attackTimer != 0)
            attackTimer = 0;

    }

    private void Attack()
    {
        if(attackTimer <= 0)
        {
            player.GetComponent<PlayerHealth>().TakeDamage(damage);
            attackTimer = attackRate;
        }

        attackTimer -= Time.deltaTime;
    }

    private bool PlayerOnRange()
    {
        float enemyToPlayerDistance = (player.transform.position - transform.position).magnitude;
        return enemyToPlayerDistance < meleeAttackDistance;
    }

    public bool HandleShooted(float damageTaken)
    {
        currentHealth -= damageTaken;
        Debug.Log("Enemy current health: " + currentHealth);
        CheckHealth();
        return true;
    }

    public bool HandleShooted()
    {
        Debug.Log("Non Damage taken");
        return true;
    }

    private void CheckHealth()
    {
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        gameObject.SetActive(false);
        // de momento lo desactivo, si nos interesa destruirlo o pasarlo por un game manager ya lo vemos.
    }
}
