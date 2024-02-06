using UnityEngine;
public class SniperWeapon : Weapon
{
    protected override void Shoot()
    {
        base.Shoot();
        var l_Ray = m_Holder.RaycastCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));
        if (Physics.Raycast(l_Ray, out RaycastHit l_RaycastHit, m_Range))
        {
            l_RaycastHit.transform.GetComponent<IShootable>()?.HandleShooted();
        }
    }
}