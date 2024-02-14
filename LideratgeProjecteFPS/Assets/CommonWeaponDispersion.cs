using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonWeaponDispersion : MonoBehaviour
{
    public float m_MinDispersion;
    public float m_MaxDispersion;
    public float m_DispPerShot;

    private Weapon m_Weapon;
    protected FPSController m_FPSController;
    protected virtual void Awake()
    {
        m_Weapon = GetComponent<Weapon>();
        m_FPSController = GetComponentInParent<FPSController>();
    }

    public virtual float GetCurrentDispersion()
    {
        return 0.0f;
    }
}

class SniperDispersion : CommonWeaponDispersion
{
    [SerializeField] private float m_WalkDispersion;
    [SerializeField] private float m_CameraDispersion;
    [SerializeField] private float m_AimingMinDispersion;
    public override float GetCurrentDispersion(Weapon weapon)
    {
        var l_Disp = 0.0f;
        if (m_FPSController.m_RigidBody.velocity.magnitude > 0.1f)
        {
            l_Disp += m_WalkDispersion;
        }

    }
}


