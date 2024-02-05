using System;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [Header("Weapon Settings")]
    [SerializeField] int m_MaxAmmo;
    [SerializeField] float m_FireRate;
    [SerializeField] float m_Damage;
    [SerializeField] float m_Range;

    [SerializeField] protected int m_CurrentAmmo;
    protected float m_LastTimeShoot;

    public static Action OnAmmoEmpty;

    private void Awake()
    {
        ResetAmmo();

    }
    private void ResetAmmo()
    {
        m_CurrentAmmo = m_MaxAmmo;
    }

    public virtual void TryShoot()
    {
        if (CanShoot())
        {
            Shoot();

            if (m_CurrentAmmo <= 0)
            {
                OnAmmoEmpty?.Invoke();
                ResetAmmo();
            }
        }
    }

    protected virtual void Shoot()
    {
        Debug.Log("Shoot " + gameObject.name);
        m_CurrentAmmo--;
        m_LastTimeShoot = Time.time;
    }

    protected virtual bool CanShoot()
    {
        return Time.time - m_LastTimeShoot >= m_FireRate;
    }

    public void SetEnabled(bool enabled)
    {
        gameObject.SetActive(enabled);
    }
}

