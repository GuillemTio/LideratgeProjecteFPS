using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DispersionHandler : MonoBehaviour
{
    private Crosshair m_Crosshair;
    private WeaponHolder m_Holder;
    
    private float m_TargetDispersion;
    private float m_CurrentDispersion;
    [SerializeField] private float m_RecoverySpeed;
    [SerializeField] float m_AddingSpeed;

    private Weapon m_EquippedWeapon;
    // Start is called before the first frame update
    private void Awake()
    {
        m_Holder = GetComponent<WeaponHolder>();
    }

    private void OnEnable()
    {
        m_Holder.OnWeaponChanged += OnWeaponChanged;
    }

    private void OnDisable()
    {
        m_Holder.OnWeaponChanged -= OnWeaponChanged;
    }

    private void OnWeaponChanged(Weapon weapon)
    {
        if (m_EquippedWeapon != null)
        {
            m_EquippedWeapon.OnShoot -= OnShoot;
        }
        weapon.OnShoot += OnShoot;
        m_EquippedWeapon = weapon;
    }

    private void OnShoot()
    {
        var l_DispPerShot = m_EquippedWeapon.GetComponent<CommonWeaponDispersion>().m_DispPerShot;
        m_CurrentDispersion += l_DispPerShot;
    }

    private void Update()
    {
        HandleDispersionModifiers();
    }

    private void HandleDispersionModifiers()
    {
        var l_DispersionW = m_EquippedWeapon.Dispersion;
        m_TargetDispersion = l_DispersionW.m_MinDispersion;
        m_TargetDispersion = GetAddedDispersion();

        m_TargetDispersion = Mathf.Clamp(m_TargetDispersion, l_DispersionW.m_MinDispersion, l_DispersionW.m_MaxDispersion);

        if (m_EquippedWeapon.IsAiming)
        {
            m_TargetDispersion = l_DispersionW.m_AimDispersion;
        }

        if (m_CurrentDispersion >= m_TargetDispersion)
            m_CurrentDispersion = Mathf.MoveTowards(m_CurrentDispersion, m_TargetDispersion, m_RecoverySpeed*Time.deltaTime);
        else if (m_CurrentDispersion <= m_TargetDispersion)
            m_CurrentDispersion = Mathf.MoveTowards(m_CurrentDispersion, m_TargetDispersion, m_AddingSpeed*Time.deltaTime);

        m_CurrentDispersion = Mathf.Clamp(m_CurrentDispersion, 0.0f, l_DispersionW.m_MaxDispersion);

        m_Holder.Crosshair.UpdateCrosshairUI(m_CurrentDispersion);
    }

    private float GetAddedDispersion()
    {
        return m_EquippedWeapon.GetComponent<CommonWeaponDispersion>().GetCurrentDispersion(m_EquippedWeapon);
    }
}
