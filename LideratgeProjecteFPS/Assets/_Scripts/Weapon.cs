using System;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] private GameObject m_MeshGameObject;
    public bool IsAiming { get; protected set; }
    public bool IsPrimary { get; protected set; }
    public WeaponHolder Holder { get; protected set; }
    public CommonWeaponDispersion Dispersion { get; protected set; }
    public Action OnDraw;
    public Action OnSeath;

    public Action OnAim;

    [Header("Weapon Settings")]
    [SerializeField] protected int m_MaxAmmo;
    [SerializeField] protected float m_FireRate;
    [SerializeField] protected float m_Damage;
    [SerializeField] protected float m_Range;
    [SerializeField] protected int m_CurrentAmmo;
    [SerializeField] protected LayerMask m_ShootableLayer;
    
    protected float m_LastTimeShoot;

    public static Action OnAmmoEmpty;
    public Action OnShoot;

    private void Awake()
    {
        ResetAmmo();
        Holder = GetComponentInParent<WeaponHolder>();
        Dispersion = GetComponent<CommonWeaponDispersion>();
        SetShowMesh(false);
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
        m_CurrentAmmo--;
        OnShoot?.Invoke();
        m_LastTimeShoot = Time.time;
    }

    protected virtual bool CanShoot()
    {
        return Time.time - m_LastTimeShoot >= m_FireRate;
    }

    public void SetShowMesh(bool enabled)
    {
        m_MeshGameObject.SetActive(enabled);
    }

    public void TryAim()
    {
        if (CanAim())
        {
            Aim();
        }
    }

    private void LateUpdate()
    {
        if (!IsPrimary) return;
        IsAiming = false;
        Debug.Log("AIMING IS FALSE:" + gameObject.name);
    }

    private void Aim()
    {
        Debug.Log("AIMED IS TRUE");
        IsAiming = true;
        OnAim?.Invoke();
    }

    private bool CanAim()
    {
        return true;
    }

    public virtual void Draw()
    {
        IsPrimary = true;
        SetShowMesh(true);
        OnDraw?.Invoke();
    }

    public virtual void Seath()
    {
        IsPrimary = false;
        SetShowMesh(false);
        OnSeath?.Invoke();
    }
}

