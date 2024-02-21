using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    [SerializeField] RectTransform m_TopLine;
    [SerializeField] RectTransform m_BotLine;
    [SerializeField] RectTransform m_LeftLine;
    [SerializeField] RectTransform m_RightLine;

    public Vector3 GetRandomPointInsideCrosshair()
    {
        float l_InsideRadius = m_TopLine.anchoredPosition.y - (m_TopLine.rect.height / 2);
        float l_RandomAngleInRadians = Random.value * 2 * Mathf.PI;
        Vector2 l_RandomDir = new Vector2(Mathf.Cos(l_RandomAngleInRadians), Mathf.Sin(l_RandomAngleInRadians));
        Vector2 l_RandomPosition = Random.value * l_InsideRadius * l_RandomDir.normalized;
        return new Vector3(l_RandomPosition.x, l_RandomPosition.y, 0.0f);
    }

    public void UpdateCrosshairUI(float dispersion)
    {
        m_TopLine.anchoredPosition = new Vector2(m_TopLine.anchoredPosition.x, dispersion + m_TopLine.rect.height / 2); 
        m_BotLine.anchoredPosition = new Vector2(m_TopLine.anchoredPosition.x, -dispersion - m_TopLine.rect.height / 2); 
        m_LeftLine.anchoredPosition = new Vector2(-dispersion - m_LeftLine.rect.width / 2, m_LeftLine.anchoredPosition.y); 
        m_RightLine.anchoredPosition = new Vector2(dispersion + m_RightLine.rect.width / 2, m_RightLine.anchoredPosition.y);
    }
}
