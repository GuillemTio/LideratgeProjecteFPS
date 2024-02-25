using System;
using System.Collections;
using UnityEngine;

public class RocketLauncherWeapon : Weapon
{
    [SerializeField] private GameObject m_RocketPrefab;
    [SerializeField] private float m_ProjectileSpeed;
    [SerializeField] private float m_BlastRadius;
    [SerializeField] private float m_SelfDamage;
    [SerializeField] private float m_ReloadTime;
    [Range(0f, 1f)]
    [SerializeField] private float m_SpeedDeboostPct;
    [SerializeField] private Transform m_RocketCreationLocation;
    [SerializeField] private KeyCode m_ReloadKeyCode = KeyCode.R;
    [SerializeField] private bool m_Reloaded;
    private bool m_Reloading;
    private bool m_CancelledReload;
    protected override void Shoot()
    {
        base.Shoot();
        var l_RocketGo = Instantiate(m_RocketPrefab);
        l_RocketGo.transform.position = m_RocketCreationLocation.position;
        var l_Rocket = l_RocketGo.GetComponent<Rocket>();
        l_Rocket.Init(this, Holder.FPSController.m_PitchController.forward, 
            m_ProjectileSpeed, m_Damage, m_SelfDamage, 
            m_BlastRadius, m_ShootableLayer);
        m_Reloaded = false;
    }
    
    private void Update()
    {
        
        if (Input.GetKeyDown(m_ReloadKeyCode) && IsPrimary)
        {
            TryReload();
        }
    }

    private void TryReload()
    {
        if (CanReload())
        {
            StartCoroutine(Reload());
        }
    }

    private bool CanReload()
    {
        return !m_Reloaded && m_CurrentAmmo > 0 && !m_Reloading;
    }

    private IEnumerator Reload()
    {
        //Play Animation
        m_CancelledReload = false;
        m_Reloading = true;
        yield return new WaitForSeconds(m_ReloadTime);
        if (!m_CancelledReload)
        {
            m_Reloaded = true;
        }
        m_Reloading = false;
    }

    public override void Seath()
    {
        base.Seath();
        m_CancelledReload = true;
        m_Reloading = false;
    }

    protected override bool CanShoot()
    {
        return m_CurrentAmmo > 0 && m_Reloaded;
    }

    public override void Draw()
    {
        base.Draw();
        if (m_CurrentAmmo == m_MaxAmmo)
        {
            m_Reloaded = true;
        }
    }
}