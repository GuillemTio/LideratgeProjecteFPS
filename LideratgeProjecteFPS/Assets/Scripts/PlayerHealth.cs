using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IHealthSystem, IShootable
{
    public float HealthFraction => Mathf.Clamp01((float)currentHealth/(float)startHealth);
    public int CurrentHealth => currentHealth;
    [SerializeField] int startHealth;
    private int currentHealth;
    [SerializeField] bool INVINCIBLE;

    public Action OnDead;
    private void Start()
    {
        currentHealth = startHealth;
    }

    public void TakeDamage(int damage)
    {
        if (!INVINCIBLE)
        {
            currentHealth -= damage;
            CheckHealth();
            Debug.Log("Player current health = " + currentHealth);
        }
    }

    public void Heal(int healing)
    {
        currentHealth += healing;
        CheckHealth();
    }

    private void CheckHealth()
    {
        if (currentHealth <= 0)
        {
            Die();
            currentHealth = 0;
        }
        else if (currentHealth > startHealth)
        {
            currentHealth = startHealth;
        }
    }

    private void Die()
    {
        Debug.Log("Player DEAD");
        OnDead?.Invoke();        
    }

    public bool HandleShooted(int damage)
    {
        TakeDamage(damage);
        return true;
    }
}
