using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Recoil : MonoBehaviour
{
    [SerializeField] private float m_MinRecoil;
    [SerializeField] private float m_MaxRecoil;
    [SerializeField] private float m_RecoilConeAnlge;
    [SerializeField] private float m_Duration;

    private Weapon m_Weapon;
    // Start is called before the first frame update
    private void Awake()
    {
        m_Weapon = GetComponent<Weapon>();
    }

    private void OnEnable()
    {
        m_Weapon.OnShoot += OnShoot;
    }

    private void OnDisable()
    {
        m_Weapon.OnShoot -= OnShoot;
    }

    private void OnShoot()
    {
        var l_RecoilAmmount = Random.Range(m_MinRecoil, m_MaxRecoil) / (m_Weapon.IsAiming? 2 : 1);

        var l_RandomRotationAngle = Random.Range(-m_RecoilConeAnlge / 2, m_RecoilConeAnlge / 2);

        Vector2 l_RecoilDir = Quaternion.AngleAxis(l_RandomRotationAngle, Vector3.forward) * Vector2.down;
        Vector2 l_Torque = l_RecoilDir.normalized * l_RecoilAmmount;

        m_Weapon.Holder.FPSController.AddTorque(l_Torque, m_Duration);
    }
}
