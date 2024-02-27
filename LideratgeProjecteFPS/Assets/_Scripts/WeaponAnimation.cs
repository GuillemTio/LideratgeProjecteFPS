using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Weapon))]
public class WeaponAnimation : MonoBehaviour
{
    [SerializeField] private float m_AimFOV;
    private Weapon m_Weapon;
    private Animator m_Animator;
    private static readonly int Shoot = Animator.StringToHash("Shoot");
    private static readonly int Aiming = Animator.StringToHash("Aiming");

    private void Awake()
    {
        m_Weapon = GetComponent<Weapon>();
        m_Animator = GetComponentInChildren<Animator>();
    }

    private void OnEnable()
    {
        Debug.Log("ENABLE ANIMATION");
        m_Weapon.OnShoot += OnShoot;
        m_Weapon.OnDraw += OnDraw;
        m_Weapon.OnSeath += OnSeath;
    }

    private void OnDisable()
    {
        m_Weapon.OnShoot -= OnShoot;
        m_Weapon.OnDraw -= OnDraw;
        m_Weapon.OnSeath -= OnSeath;
    }
    private void OnSeath()
    {
        m_Animator.SetBool(Aiming, false);      
    }

    private void OnDraw()
    {
        Debug.Log("SET TARGET FOV");
        CameraAiming.Instance.TargetFOV = m_AimFOV;
    }

    private void Update()
    {
        if (!m_Weapon.IsPrimary)
        {
            return;
        }
        Debug.Log("UPDATING ANIMATION: " + m_Weapon.gameObject.name);
        CameraAiming.Instance.Aiming = m_Weapon.IsAiming;
        m_Animator.SetBool(Aiming, m_Weapon.IsAiming);
    }

    private void OnShoot()
    {
        m_Animator.SetTrigger(Shoot);
    }
}
