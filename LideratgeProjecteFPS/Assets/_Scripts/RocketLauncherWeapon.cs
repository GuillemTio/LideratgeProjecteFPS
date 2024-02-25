using System;
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
    protected override void Shoot()
    {
        base.Shoot();
        var l_RocketGo = Instantiate(m_RocketPrefab);
        l_RocketGo.transform.position = m_RocketCreationLocation.position;
        var l_Rocket = l_RocketGo.GetComponent<Rocket>();
        l_Rocket.Init(this, Holder.FPSController.m_PitchController.forward, 
            m_ProjectileSpeed, m_Damage, m_SelfDamage, 
            m_BlastRadius, m_ShootableLayer);
    }
}