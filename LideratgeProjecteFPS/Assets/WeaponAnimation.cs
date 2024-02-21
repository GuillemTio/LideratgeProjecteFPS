using System;
using System.Collections;
using System.Collections.Generic;
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
        m_Animator = GetComponentInChildren<Animator>();
        m_Weapon = GetComponent<Weapon>();
    }

    private void OnEnable()
    {
        m_Weapon.OnShoot += OnShoot;
        m_Weapon.OnDraw += Draw;
        m_Weapon.OnUndraw += Undraw;
    }

    private void OnDisable()
    {
        m_Weapon.OnShoot -= OnShoot;
        m_Weapon.OnDraw -= Draw;
        m_Weapon.OnUndraw -= Undraw;
    }
    private void Undraw()
    {
        
    }

    private void Draw()
    {
        Debug.Log("SET TARGET FOV");
        CameraAiming.TargetFOV = m_AimFOV;
    }

    private void Update()
    {
        m_Animator.SetBool(Aiming, m_Weapon.IsAiming);
        CameraAiming.Aiming = m_Weapon.IsAiming;
    }

    private void OnShoot()
    {
        m_Animator.SetTrigger(Shoot);
    }
}
