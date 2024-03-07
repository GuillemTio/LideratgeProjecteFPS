using System;
using UnityEngine;

class SimpleEnemy : MonoBehaviour, IShootable, IHealthSystem
{
    [SerializeField] int startHealth;
    private int currentHealth;

    [SerializeField] private GameObject m_EnemyExplosion;
    [SerializeField] int damage;
    [SerializeField] float meleeAttackDistance;
    [SerializeField] float attackRate;
    private float attackTimer;
    GameObject player;

    public Animator animator;

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
            animator.SetTrigger("Attack");
            attackTimer = attackRate;
        }

        attackTimer -= Time.deltaTime;
    }

    private bool PlayerOnRange()
    {
        float enemyToPlayerDistance = (player.transform.position - transform.position).magnitude;
        return enemyToPlayerDistance < meleeAttackDistance;
    }

    public bool HandleShooted(int damage)
    {
        currentHealth -= damage;
        CheckHealth();
        return true;
    }

    private void CheckHealth()
    {
        if (currentHealth <= 0)
        {
            Die();
            currentHealth = 0;
        }
    }

    public void DieAnimationEnd()
    {
        gameObject.SetActive(false);
        Instantiate(m_EnemyExplosion, transform.position + Vector3.up * 1, Quaternion.identity);
        // de momento lo desactivo, si nos interesa destruirlo o pasarlo por un game manager ya lo vemos.
    }

    private void Die()
    {
        animator.SetTrigger("Dead");
        GameObject.FindGameObjectWithTag("PointsManager").GetComponent<PointsManager>().EnemyKilled();
    }

    public bool HandleShooted(float damage)
    {
        return true;
    }

    public float HealthFraction => Mathf.Clamp01((float)currentHealth/(float)startHealth);
    public int CurrentHealth => currentHealth;
}
