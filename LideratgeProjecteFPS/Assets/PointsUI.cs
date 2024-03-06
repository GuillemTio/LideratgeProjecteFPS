using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointsUI : MonoBehaviour
{
    [SerializeField] private Text m_Text;

    private PointsManager m_PointsManager;

    private void Awake()
    {
        m_PointsManager = GetComponentInParent<PointsManager>();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        m_Text.text = m_PointsManager.CurrentPoints.ToString();
    }
}
