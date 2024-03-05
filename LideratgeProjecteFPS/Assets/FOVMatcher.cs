using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOVMatcher : MonoBehaviour
{
    private Camera m_Camera;
    private Camera m_MainCamera;
    void Start()
    {
        m_Camera = GetComponent<Camera>();
        m_MainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        m_Camera.fieldOfView = m_MainCamera.fieldOfView;
    }
}
