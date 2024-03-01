using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] float startHealth;
    private float currentHealth;
    [SerializeField] bool INVINCIBLE;

    private void Start()
    {
        currentHealth = startHealth;
    }

    public void TakeDamage(float damage)
    {
        if (!INVINCIBLE)
        {
            currentHealth -= damage;
            CheckHealth();
            Debug.Log("Player current health = " + currentHealth);
        }
    }

    public void Heal(float healing)
    {
        currentHealth += healing;
        CheckHealth();
        Debug.Log("Player current health = " + currentHealth);
    }

    private void CheckHealth()
    {
        if (currentHealth <= 0)
        {
            Die();
        }
        else if (currentHealth > startHealth)
        {
            currentHealth = startHealth;
        }
    }

    private void Die()
    {
        Debug.Log("Player DEAD");

    }
}
