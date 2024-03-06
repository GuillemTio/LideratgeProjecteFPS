using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BloodSwordWeapon : Weapon
{
    public Action<bool> OnBuffedChanged;
    public Action OnBuffedAttack;
    public ParticleSystem m_Particles;
    
    [Header("Healing Settings")]
    [SerializeField] private int m_HealingPerShot;
    private PlayerHealth m_PlayerHealth;

    [Header("Buff Settings")] 
    [SerializeField] private GameObject m_ProjectilePrefab;
    [SerializeField] private float m_TimeOfBuff;
    [SerializeField] private KeyCode m_BuffKeyCode = KeyCode.Mouse1;
    [SerializeField] private int m_BuffedAttackAmmo = 3;
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
    private float m_LastBuffAttack;

    protected override void Awake()
    {
        base.Awake();
        m_PlayerHealth = GetComponentInParent<PlayerHealth>();
    }

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
        m_LastBuffAttack = Time.time;
        OnBuffedChanged?.Invoke(false);
        OnBuffedAttack?.Invoke();
        var l_BubbleGo = Instantiate(m_ProjectilePrefab);
        l_BubbleGo.transform.position = transform.position;
        var l_Rocket = l_BubbleGo.GetComponent<BloodBubble>();
        l_Rocket.Init(this, Holder.FPSController.m_PitchController.forward, 
            m_BubbleSpeed, m_Damage, m_ShootableLayer);
        m_Buffed = false;
        if (m_CurrentAmmo <= 0)
        {
            OnAmmoEmpty?.Invoke();
            ResetAmmo();
        }
    }

    private void Update()
    {
        if (Input.GetKey(m_BuffKeyCode))
        {
            if (!IsBuffing && !m_Buffed && m_CurrentAmmo > m_BuffedAttackAmmo)
            {
                StartCoroutine(BuffSword());
            }
        }
    }

    private IEnumerator BuffSword()
    {
        IsBuffing = true;
        var l_Timer = m_TimeOfBuff;
        while (l_Timer > 0 && Input.GetKey(m_BuffKeyCode) && IsPrimary)
        {
            l_Timer -= Time.deltaTime;
            yield return null;
        }
        if (l_Timer <= 0)
        {
            m_Buffed = true;
            OnBuffedChanged?.Invoke(true);
            m_CurrentAmmo -= m_BuffedAttackAmmo;
        }
        IsBuffing = false;
    }

    private void Attack()
    {
        BeforeShoot();
        Invoke(nameof(Shoot), m_ShootDelay);
        IsOnAttack = true;
        m_LastAttackTime = Time.time;
    }

    protected override void Shoot()
    {
        base.Shoot();
        var l_HolderTransform = Holder.transform;
        var l_Forward = l_HolderTransform.forward;
        var l_Hits = Physics.SphereCastAll(l_HolderTransform.position + l_Forward * m_Range,
            m_SphereCastRadius, l_Forward, m_SphereCastRadius*2, m_ShootableLayer);
        if (l_Hits.Length == 0)
            return;
        m_Particles.Play();
        foreach (var l_Hit in l_Hits)
        {
            Debug.Log(l_Hit.point);
            if (l_Hit.transform.TryGetComponent(out IShootable l_Shootable))
            {
                l_Shootable.HandleShooted(m_Damage);
                m_PlayerHealth.Heal(m_HealingPerShot);
            }
        }
    }

    public override bool CanShoot()
    {
        return base.CanShoot() && !IsOnAttack && !IsBuffing && Time.time - m_LastBuffAttack > 0.5f;
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
