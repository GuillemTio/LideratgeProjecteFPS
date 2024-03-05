using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Weapon))]
public class WeaponAnimation : MonoBehaviour
{
    [SerializeField] private float m_AimFOV;
    protected Weapon m_Weapon;
    protected Animator m_Animator;
    private static readonly int Shoot = Animator.StringToHash("Shoot");
    private static readonly int Aiming = Animator.StringToHash("Aiming");
    private static readonly int Seath = Animator.StringToHash("Seath");
    private static readonly int Draw = Animator.StringToHash("Draw");

    protected virtual void Awake()
    {
        m_Weapon = GetComponent<Weapon>();
        m_Animator = GetComponentInChildren<Animator>();
    }

    protected virtual void OnEnable()
    {
        Debug.Log("ENABLE ANIMATION");
        m_Weapon.OnShoot += OnShoot;
        m_Weapon.OnDraw += OnDraw;
        m_Weapon.OnSeath += OnSeath;
    }

    protected virtual void OnDisable()
    {
        m_Weapon.OnShoot -= OnShoot;
        m_Weapon.OnDraw -= OnDraw;
        m_Weapon.OnSeath -= OnSeath;
    }
    protected virtual void OnSeath()
    {
        m_Animator.SetBool(Aiming, false);
        CameraAiming.Instance.Aiming = false;
        m_Animator.SetTrigger(Seath);
    }

    protected virtual void OnDraw()
    {
        // Debug.Log("SET TARGET FOV");
        m_Animator.SetTrigger(Draw);
        CameraAiming.Instance.TargetFOV = m_AimFOV;
    }

    protected virtual void Update()
    {
        if (!m_Weapon.IsPrimary)
        {
            return;
        }
        // Debug.Log("UPDATING ANIMATION: " + m_Weapon.gameObject.name);
        CameraAiming.Instance.Aiming = m_Weapon.IsAiming;
        m_Animator.SetBool(Aiming, m_Weapon.IsAiming);
    }

    protected virtual void OnShoot()
    {
        m_Animator.SetTrigger(Shoot);
    }
}