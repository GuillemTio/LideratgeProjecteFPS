using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonWeaponDispersion : MonoBehaviour
{
    public virtual float DispersionPerShot => m_DispPerShot;
    public float m_MinDispersion;
    public float m_MaxDispersion;
    private float m_DispPerShot;
    public float m_AimDispersion;

    protected Weapon m_Weapon;
    protected FPSController m_FPSController;
    protected virtual void Awake()
    {
        m_Weapon = GetComponent<Weapon>();
        m_FPSController = GetComponentInParent<FPSController>();
    }

    public virtual float GetCurrentDispersion(Weapon weapon)
    {
        return 0.0f;
    }
}