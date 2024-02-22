using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class WeaponHolder : MonoBehaviour
{
    public FPSController FPSController { get; private set; }
    public Camera RaycastCam => m_RaycastCam;
    public Crosshair Crosshair => m_Crosshair;
    
    [SerializeField] private Camera m_RaycastCam;
    [SerializeField] private Crosshair m_Crosshair;
    
    [Header("Input")]
    [SerializeField] private KeyCode m_ShootKeyCode = KeyCode.Mouse0;
    [SerializeField] private KeyCode m_AimKeyCode = KeyCode.Mouse1;
    [SerializeField] private KeyCode m_ChangeWeaponKeyCode = KeyCode.Q;

    [SerializeField] private string m_NextWeaponName;
    List<Weapon> m_WeaponList;

    WeaponPair m_Pair;
    Queue<Weapon> m_BackWeapons = new();
    public Action<Weapon> OnWeaponChanged;

    private void Awake()
    {
        FPSController = GetComponentInParent<FPSController>();
    }

    void Start()
    {
        m_WeaponList = GetComponentsInChildren<Weapon>().ToList<Weapon>();
        SetEnabledAllWeapons(false);
        m_Pair = new(m_WeaponList);

        for (int i = 2; i < m_WeaponList.Count; i++)
        {
            Weapon l_Weapon = m_WeaponList[i];
            m_BackWeapons.Enqueue(l_Weapon);
        }
        m_NextWeaponName = m_BackWeapons.Peek().gameObject.name;

        OnWeaponChanged?.Invoke(m_Pair.PrimaryWeapon);
        m_Pair.PrimaryWeapon.Draw();
    }
    private void OnEnable()
    {
        Weapon.OnAmmoEmpty += OnAmmoEmpty;
    }
    private void OnDisable()
    {
        Weapon.OnAmmoEmpty -= OnAmmoEmpty;
    }

    private void SetEnabledAllWeapons(bool enabled)
    {
        foreach (var weapon in m_WeaponList)
        {
            weapon.SetEnabled(enabled);
        }
    }

    private void OnAmmoEmpty()
    {
        LoadNextWeapon();
        
    }

    private void LoadNextWeapon()
    {
        SetEnabledAllWeapons(false);

        m_BackWeapons.Enqueue(m_Pair.PrimaryWeapon);
        m_Pair.PrimaryWeapon = m_BackWeapons.Dequeue();

        m_Pair.PrimaryWeapon.SetEnabled(true);
        m_NextWeaponName = m_BackWeapons.Peek().gameObject.name;
        OnWeaponChanged?.Invoke(m_Pair.PrimaryWeapon);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(m_ChangeWeaponKeyCode))
        {
            ChangeWeapon();
            return;
        }
        if (Input.GetKey(m_ShootKeyCode))
        {
            TryShootWeapon();
        }

        
        if (Input.GetKey(m_AimKeyCode))
        {
            TryAim();
        }
    }

    private void TryAim()
    {
        m_Pair.PrimaryWeapon.TryAim();
    }

    private void ChangeWeapon()
    {
        m_Pair.SwapWeapons();
        OnWeaponChanged?.Invoke(m_Pair.PrimaryWeapon);
    }

    private void TryShootWeapon()
    {
        m_Pair.PrimaryWeapon.TryShoot();
    }
}
