using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAiming : MonoBehaviour
{
    public static CameraAiming Instance;
    public bool Aiming;
    public float TargetFOV;
    private Camera m_Camera;

    [Range(0f, .5f)]
    [SerializeField] private float m_LerpValue;

    private float m_StartFOV;

    // Start is called before the first frame update
    private void Awake()
    {
        m_Camera = GetComponent<Camera>();
        Instance = this;
    }

    void Start()
    {
        m_StartFOV = m_Camera.fieldOfView;
    }

    // Update is called once per frame

    private void Update()
    {
        // Debug.Log("UPDATE CAMERA");
        if (Aiming)
        {
            m_Camera.fieldOfView = Mathf.Lerp(m_Camera.fieldOfView, TargetFOV, m_LerpValue);
        }
        else
        {
            m_Camera.fieldOfView = Mathf.Lerp(m_Camera.fieldOfView, m_StartFOV, m_LerpValue);
        }
    }
}
