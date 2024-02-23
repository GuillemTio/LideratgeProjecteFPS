    using UnityEngine;

public class Rocket : MonoBehaviour
{
    private Weapon m_Weapon;
    private Rigidbody m_Rb;
    private float m_Damage;
    private float m_DamagePlayer;
    private float m_BlastRadius;
    private LayerMask m_ShootableLayer;

    private void Awake()
    {
        m_Rb = GetComponent<Rigidbody>();
        m_Rb.isKinematic = true;
        m_Rb.useGravity = false;
    }

    public void Init(Weapon weapon, Vector3 forward, float speed, float damage, float damagePlayer, 
        float blastRadius, LayerMask shootableLayer)
    {
        m_Weapon = weapon;
        m_Rb.isKinematic = false;
        m_Rb.velocity = speed * forward;
        m_Damage = damage;
        m_DamagePlayer = damagePlayer;
        m_BlastRadius = blastRadius;
        m_ShootableLayer = shootableLayer;
        Physics.IgnoreCollision(GetComponent<Collider>(), m_Weapon.Holder.FPSController.GetComponentInChildren<Collider>());
    }

    private void OnTriggerEnter(Collider other)
    {
        Explode();
    }

    private void Explode()
    {
        var l_Hits = Physics.SphereCastAll(transform.position,
            m_BlastRadius, transform.forward, m_BlastRadius, m_ShootableLayer);
        if (l_Hits.Length == 0)
            return;
        foreach (var l_Hit in l_Hits)
        {
            if (l_Hit.transform == m_Weapon.Holder.FPSController.transform)
            {
                l_Hit.transform.GetComponent<IShootable>()?.HandleShooted(m_DamagePlayer);
            }
            else
            {
                l_Hit.transform.GetComponent<IShootable>()?.HandleShooted(m_Damage);
            }
        }
        
        Destroy(gameObject);
    }
}