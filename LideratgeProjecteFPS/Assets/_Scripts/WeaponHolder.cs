using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class WeaponHolder : MonoBehaviour
{
    public Weapon PrimaryWeapon => m_Pair.PrimaryWeapon;
    public Weapon SecondaryWeapon => m_Pair.SecondaryWeapon;
    public Weapon NextWeapon => m_BackWeapons.Peek();
    public FPSController FPSController { get; private set; }
    public Camera RaycastCam => m_RaycastCam;
    public Crosshair Crosshair => m_Crosshair;

    public CPoolElements MetaldDecalPool;
    public CPoolElements StoneDecalPool;
    [SerializeField] private GameObject m_MetalDecal;
    [SerializeField] private GameObject m_StoneDecal;
    
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

    public bool m_HasChangedThisFrame;
    private void Awake()
    {
        FPSController = GetComponentInParent<FPSController>();
        MetaldDecalPool = new CPoolElements(m_MetalDecal, 10, null);
        StoneDecalPool = new CPoolElements(m_StoneDecal, 10, null);
    }

    void Start()
    {
        m_WeaponList = GetComponentsInChildren<Weapon>().ToList<Weapon>();
        m_Pair = new(m_WeaponList);

        for (int i = 2; i < m_WeaponList.Count; i++)
        {
            Weapon l_Weapon = m_WeaponList[i];
            m_BackWeapons.Enqueue(l_Weapon);
        }
        m_NextWeaponName = m_BackWeapons.Peek().gameObject.name;

        OnWeaponChanged?.Invoke(m_Pair.PrimaryWeapon);
        UpdatePairState(null);
    }

    private void UpdatePairState(Weapon seathedWeapon)
    {
        StopAllCoroutines();
        if (seathedWeapon == null)
        {
            m_Pair.PrimaryWeapon.Draw();
        }
        else
        {
            m_Pair.PrimaryWeapon.InTransition = true;
            m_Pair.SecondaryWeapon.InTransition = true;
            StartCoroutine(DrawWhenOtherSeathed(seathedWeapon));
        }
    }

    private IEnumerator DrawWhenOtherSeathed(Weapon seathedWeapon)
    {
        yield return new WaitWhile(() => seathedWeapon.InTransition);
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

    private void OnAmmoEmpty()
    {
        LoadNextWeapon();
    }

    private void LoadNextWeapon()
    {
        m_HasChangedThisFrame = true;
        var l_SeathedWeapon = m_Pair.PrimaryWeapon;
        m_Pair.PrimaryWeapon.Seath();
        m_BackWeapons.Enqueue(m_Pair.PrimaryWeapon);
        m_Pair.PrimaryWeapon = m_BackWeapons.Dequeue();
        m_NextWeaponName = m_BackWeapons.Peek().gameObject.name;
        OnWeaponChanged?.Invoke(m_Pair.PrimaryWeapon);
        UpdatePairState(l_SeathedWeapon);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(m_AimKeyCode))
        {
            TryAim();
        }
        if (Input.GetKeyDown(m_ChangeWeaponKeyCode))
        {
            ChangeWeapon();
            return;
        }
        if (Input.GetKey(m_ShootKeyCode))
        {
            TryShootWeapon();
        }
    }

    private void TryAim()
    {
        m_Pair.PrimaryWeapon.TryAim();
    }

    private void ChangeWeapon()
    {
        m_HasChangedThisFrame = true;
        m_Pair.SwapWeapons();
        OnWeaponChanged?.Invoke(m_Pair.PrimaryWeapon);
        m_Pair.SecondaryWeapon.Seath();
        UpdatePairState(m_Pair.SecondaryWeapon);
    }

    private void TryShootWeapon()
    {
        // if (m_HasChangedThisFrame)
        // {
        //     m_HasChangedThisFrame = false;
        //     return;
        // }
        m_Pair.PrimaryWeapon.TryShoot();
    }
}
