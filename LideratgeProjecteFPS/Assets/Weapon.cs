using System;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    protected WeaponHolder m_Holder;

    [Header("OldWeapon Settings")]
    [SerializeField] protected int m_MaxAmmo;
    [SerializeField] protected float m_FireRate;
    [SerializeField] protected float m_Damage;
    [SerializeField] protected float m_Range;
    [SerializeField] protected int m_CurrentAmmo;
    //[SerializeField] protected bool m_HasDispersion;
    //[SerializeField] protected bool m_HasRecoil;

    protected float m_LastTimeShoot;

    public static Action OnAmmoEmpty;
    public Action OnShoot;

    private void Awake()
    {
        ResetAmmo();
        m_Holder = GetComponentInParent<WeaponHolder>();
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
        OnShoot?.Invoke();
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

