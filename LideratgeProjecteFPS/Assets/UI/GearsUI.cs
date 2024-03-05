using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GearsUI : MonoBehaviour
{
    [Header("Scene References")] 
    [SerializeField] private Text m_AmmoText;
    [SerializeField] private Image m_SecondaryWpIcon;
    [SerializeField] private Image m_NextWpIcon;
    [Space]
    [SerializeField] private WeaponHolder m_Holder;
    
    [Header("Resources")] 
    [SerializeField] private Sprite m_SniperIcon;
    [SerializeField] private Color m_SniperColor;
    [SerializeField] private Sprite m_RLauncherIcon;
    [SerializeField] private Color m_RLauncherColor;
    [SerializeField] private Sprite m_BSwordIcon;
    [SerializeField] private Color m_BSwordColor;

    private Animator m_Animator;
    private static readonly int Spin = Animator.StringToHash("Spin");

    private void Awake()
    {
        m_Animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        m_Holder.OnWeaponChanged += UpdateUIIcons;
    }
    private void OnDisable()
    {
        m_Holder.OnWeaponChanged -= UpdateUIIcons;
    }

    private void UpdateUIIcons(Weapon primaryWp)
    {
        var secondaryWp = m_Holder.SecondaryWeapon;
        var nextWp = m_Holder.NextWeapon;

        m_SecondaryWpIcon.sprite = GetIcon(secondaryWp);
        m_NextWpIcon.sprite = GetIcon(nextWp);

        m_AmmoText.color = GetColor(primaryWp);
        m_Animator.SetTrigger(Spin);
    }


    private void Update()
    {
        UpdateUIAmmo();
    }

    private Sprite GetIcon(Weapon wp)
    {
        return wp switch
        {
            SniperWeapon => m_SniperIcon,
            RocketLauncherWeapon => m_RLauncherIcon,
            _ => m_BSwordIcon
        };
    }
    
    private Color GetColor(Weapon wp)
    {
        return wp switch
        {
            SniperWeapon => m_SniperColor,
            RocketLauncherWeapon => m_RLauncherColor,
            _ => m_BSwordColor
        };
    }

    private void UpdateUIAmmo()
    {
        var l_AmmoText = m_Holder.PrimaryWeapon.CurrentAmmo.ToString();
        if (l_AmmoText.Length == 1)
        {
            l_AmmoText = "0" + l_AmmoText;
        }
        m_AmmoText.text = l_AmmoText;
    }
}
