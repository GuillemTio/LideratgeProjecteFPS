using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodBubble : MonoBehaviour
{
    private Weapon m_Weapon;
    private Rigidbody m_Rb;
    private float m_Damage;
    private LayerMask m_ShootableLayerMask;
    private void Awake()
    {
        m_Rb = GetComponent<Rigidbody>();
        m_Rb.isKinematic = true;
        m_Rb.useGravity = false;
    }

    public void Init(Weapon weapon, Vector3 forward, float speed, float damage, LayerMask shootableLayer)
    {
        m_Weapon = weapon;
        m_Rb.isKinematic = false;
        m_Rb.velocity = speed * forward;
        m_Damage = damage;
        m_ShootableLayerMask = shootableLayer;
        Physics.IgnoreCollision(GetComponent<Collider>(), m_Weapon.Holder.FPSController.GetComponentInChildren<Collider>());
    }
    private void OnTriggerEnter(Collider other)
    {
        if (( m_ShootableLayerMask & (1 << other.gameObject.layer)) != 0)
        {
            other.GetComponent<IShootable>()?.HandleShooted(m_Damage);
        }
        Destroy(gameObject);
    }
}
