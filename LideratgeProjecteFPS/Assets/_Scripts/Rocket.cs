    using Unity.Mathematics;
    using UnityEngine;

public class Rocket : MonoBehaviour
{
    private Weapon m_Weapon;
    private Rigidbody m_Rb;
    private int m_Damage;
    private int m_DamagePlayer;
    private float m_BlastRadius;
    private LayerMask m_ShootableLayer;
    private GameObject m_Decal;

    private void Awake()
    {
        m_Rb = GetComponent<Rigidbody>();
        m_Rb.isKinematic = true;
        m_Rb.useGravity = false;
    }

    public void Init(Weapon weapon, Vector3 forward, float speed, int damage, int damagePlayer, 
        float blastRadius, LayerMask shootableLayer, GameObject decal)
    {
        m_Weapon = weapon;
        m_Rb.isKinematic = false;
        m_Rb.velocity = speed * forward;
        m_Damage = damage;
        m_DamagePlayer = damagePlayer;
        m_BlastRadius = blastRadius;
        m_ShootableLayer = shootableLayer;
        m_Decal = decal;
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

        Instantiate(m_Decal, transform.position, quaternion.identity);
        Destroy(gameObject);
    }
}