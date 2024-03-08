using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodSwordAnimation : WeaponAnimation
{
    private new BloodSwordWeapon m_Weapon;
    private static readonly int Charging = Animator.StringToHash("Charging");
    private static readonly int ShootID = Animator.StringToHash("ShootID");

    protected override void OnEnable()
    {
        base.OnEnable();
        m_Weapon.OnBuffedAttack += OnBuffedAttack;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        m_Weapon.OnBuffedAttack -= OnBuffedAttack;
    }

    private void OnBuffedAttack()
    {
        base.OnShoot();
        m_Animator.SetInteger(ShootID, 0);
    }

    protected override void Awake()
    {
        base.Awake();
        m_Weapon = GetComponent<BloodSwordWeapon>();
    }

    protected override void OnShoot()
    {
        base.OnShoot();
        m_Animator.SetInteger(ShootID, m_Weapon.CurrentShootID);
    }
}
