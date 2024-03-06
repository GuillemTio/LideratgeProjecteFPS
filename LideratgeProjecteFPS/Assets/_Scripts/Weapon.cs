using System;
using System.Collections;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] private GameObject m_MeshGameObject;
    public bool IsAiming { get; protected set; }
    public bool IsPrimary { get; protected set; }
    public WeaponHolder Holder { get; protected set; }
    public int CurrentAmmo => m_CurrentAmmo;
    public CommonWeaponDispersion Dispersion { get; protected set; }
    public Action OnDraw;
    public Action OnSeath;
    public Action OnAim;

    [Header("Weapon Settings")]
    [SerializeField] protected int m_MaxAmmo;
    [SerializeField] protected float m_FireRate;
    [SerializeField] protected int m_Damage;
    [SerializeField] protected float m_Range;
    [SerializeField] protected int m_CurrentAmmo;
    [SerializeField] protected float m_ShootDelay;
    [SerializeField] protected LayerMask m_ShootableLayer;
    
    protected float m_LastTimeShoot;

    public static Action OnAmmoEmpty; 
    public Action OnShoot;
    public bool InTransition;

    protected virtual void Awake()
    {
        ResetAmmo();
        Holder = GetComponentInParent<WeaponHolder>();
        Dispersion = GetComponent<CommonWeaponDispersion>();
        SetShowMesh(false);
    }
    protected void ResetAmmo()
    {
        m_CurrentAmmo = m_MaxAmmo;
    }
    
    public virtual void TryShoot()
    {
        if (CanShoot())
        {
            BeforeShoot();
            Invoke(nameof(Shoot), m_ShootDelay);
        }
    }

    protected virtual void BeforeShoot()
    {
        OnShoot?.Invoke();
        m_LastTimeShoot = Time.time;
        m_CurrentAmmo--;
    }
    protected virtual void Shoot()
    {
        if (m_CurrentAmmo <= 0)
        {
            OnAmmoEmpty?.Invoke();
            ResetAmmo();
        }
    }

    public virtual bool CanShoot()
    {
        return Time.time - m_LastTimeShoot >= m_FireRate && !InTransition;
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
        // Debug.Log("AIMING IS FALSE:" + gameObject.name);
    }

    private void Aim()
    {
        // Debug.Log("AIMED IS TRUE");
        IsAiming = true;
        OnAim?.Invoke();
    }

    protected virtual bool CanAim()
    {
        return !InTransition;
    }

    public virtual void Draw()
    {
        IsPrimary = true;
        InTransition = true;
        OnDraw?.Invoke();
        // SetShowMesh(true);
    }

    private IEnumerator HideWhenSeathed()
    {
        yield return new WaitWhile(()=>InTransition);
        SetShowMesh(false);
    }

    public virtual void Seath()
    {
        IsPrimary = false;
        InTransition = true;
        OnSeath?.Invoke();
        // StartCoroutine(HideWhenSeathed());
    }
}

