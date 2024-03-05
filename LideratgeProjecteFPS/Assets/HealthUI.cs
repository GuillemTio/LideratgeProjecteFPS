using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [Header("Scene References")] 
    [SerializeField] private Slider m_Slider;
    [SerializeField] private Text m_Health;

    [SerializeField] private Gradient m_Gradient;
    private PlayerHealth m_PlayerHealth;

    private void Awake()
    {
        m_PlayerHealth = GetComponentInParent<PlayerHealth>();
    }

    private void Update()
    {
        m_Slider.value = m_PlayerHealth.HealthFraction;
        m_Health.text = m_PlayerHealth.CurrentHealth.ToString();
        m_Slider.fillRect.GetComponent<Image>().color = m_Gradient.Evaluate(m_PlayerHealth.HealthFraction);
    }
}
