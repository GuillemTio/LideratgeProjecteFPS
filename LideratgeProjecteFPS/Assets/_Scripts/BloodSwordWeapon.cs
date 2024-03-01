using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BloodSwordWeapon : Weapon
{
    public Action<bool> OnBuffedChanged;
    public Action OnBuffedAttack;
    
    [Header("Healing Settings")]
    [SerializeField] private float m_HealingPerShot;

    [Header("Buff Settings")] 
    [SerializeField] private GameObject m_ProjectilePrefab;
    [SerializeField] private float m_TimeOfBuff;
    [SerializeField] private KeyCode m_BuffKeyCode = KeyCode.Mouse1;
    public bool m_Buffed;
    public bool IsBuffing;
    [Space] 
    [SerializeField] private float m_BubbleSpeed;
    
    
    [Header("Attack Settings")]
    [SerializeField] private float m_ComboAttackTimeBuffer;
    [SerializeField] private float m_SphereCastRadius;
    public bool IsOnAttack;
    public int CurrentShootID;
    private float m_LastAttackTime;

    public override void TryShoot()
    {
        if (CanBuffAttack())
        {
            BuffAttack();
        }
        else if (CanShoot())
        {
            if (!MustAttackCombo())
                CurrentShootID = 0;
            Attack();
            ++CurrentShootID;
            if (CurrentShootID > 1)
                CurrentShootID = 0;
        }
    }

    private bool CanBuffAttack()
    {
        return IsAiming && m_Buffed;
    }

    private void BuffAttack()
    {
        OnBuffedChanged?.Invoke(false);
        OnBuffedAttack?.Invoke();
        var l_BubbleGo = Instantiate(m_ProjectilePrefab);
        l_BubbleGo.transform.position = transform.position;
        var l_Rocket = l_BubbleGo.GetComponent<BloodBubble>();
        l_Rocket.Init(this, Holder.FPSController.m_PitchController.forward, 
            m_BubbleSpeed, m_Damage, m_ShootableLayer);
        m_Buffed = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(m_BuffKeyCode))
        {
            if (!IsBuffing && !m_Buffed)
            {
                StartCoroutine(BuffSword());
            }
        }
    }

    private IEnumerator BuffSword()
    {
        IsBuffing = true;
        var l_Timer = m_TimeOfBuff;
        while (l_Timer > 0 && Input.GetKey(m_BuffKeyCode))
        {
            l_Timer -= Time.deltaTime;
            yield return null;
        }
        if (l_Timer <= 0)
        {
            m_Buffed = true;
            OnBuffedChanged?.Invoke(true);
            m_CurrentAmmo -= 3;
        }
        IsBuffing = false;
    }

    private void Attack()
    {
        Debug.Log("HERE");
        BeforeShoot();
        Invoke(nameof(Shoot), m_ShootDelay);
        IsOnAttack = true;
        m_LastAttackTime = Time.time;
    }

    protected override void Shoot()
    {
        base.Shoot();
        var l_Hits = Physics.SphereCastAll(transform.position + transform.forward * m_Range,
            m_SphereCastRadius, transform.forward, m_SphereCastRadius, m_ShootableLayer);
        if (l_Hits.Length == 0)
            return;
        foreach (var l_Hit in l_Hits)
        {
            if (l_Hit.transform != Holder.FPSController.transform)
            {
                l_Hit.transform.GetComponent<IShootable>()?.HandleShooted(m_Damage);
            }
        }
    }

    public override bool CanShoot()
    {
        return base.CanShoot() && !IsOnAttack;
    }

    protected override bool CanAim()
    {
        return base.CanAim() && m_Buffed;
    }

    private bool MustAttackCombo()
    {
        return (Time.time - m_LastAttackTime) < m_ComboAttackTimeBuffer;
    }
}
