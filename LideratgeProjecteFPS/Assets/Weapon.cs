﻿using System;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public bool IsAiming { get; protected set; }
    public WeaponHolder Holder { get; protected set; }
    public CommonWeaponDispersion Dispersion { get; protected set; }
    public Action OnDraw;
    public Action OnUndraw;

    public Action OnAim;

    
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
        Holder = GetComponentInParent<WeaponHolder>();
        Dispersion = GetComponent<CommonWeaponDispersion>();
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

    public void TryAim()
    {
        Aim();
    }

    private void LateUpdate()
    {
        IsAiming = false;
    }

    private void Aim()
    {
        IsAiming = true;
        OnAim?.Invoke();
    }

    private bool CanAim()
    {
        return true;
    }

    public void Draw()
    {
        OnDraw?.Invoke();
    }

    public void Undraw()
    {
        OnUndraw?.Invoke();
    }
}

