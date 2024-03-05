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
    private IHealthSystem m_HealthSystem;

    private void Awake()
    {
        m_HealthSystem = GetComponentInParent<IHealthSystem>();
    }

    private void Update()
    {
        m_Slider.value = m_HealthSystem.HealthFraction;
        if (m_Health != null)
        {
            m_Health.text = m_HealthSystem.CurrentHealth.ToString();
        }
        m_Slider.fillRect.GetComponent<Image>().color = m_Gradient.Evaluate(m_HealthSystem.HealthFraction);
    }
}
