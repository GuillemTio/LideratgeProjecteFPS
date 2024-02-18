using System;
using Unity.VisualScripting;
using UnityEngine;
public class SniperWeapon : Weapon
{
    public bool IsAiming { get; set; }

    private void Update()
    {
        IsAiming = Input.GetKey(KeyCode.Mouse1);
    }

    protected override void Shoot()
    {
        base.Shoot();
        
        Vector3 l_CameraCenter = new(0.50f, 0.5f, 0.0f);
        Vector3 l_DispersionOffset = m_Holder.Crosshair.GetRandomPointInsideCrosshair();
        
        l_DispersionOffset.x /= m_Holder.RaycastCam.pixelWidth;
        l_DispersionOffset.y /= m_Holder.RaycastCam.pixelHeight;
        
        var l_Ray = m_Holder.RaycastCam.ViewportPointToRay(l_CameraCenter + l_DispersionOffset);
        
        if (Physics.Raycast(l_Ray, out RaycastHit l_RaycastHit, m_Range))
        {
            l_RaycastHit.transform.GetComponent<IShootable>()?.HandleShooted();
            Debug.DrawLine(l_RaycastHit.point, l_RaycastHit.point + l_RaycastHit.normal, Color.red, 5f);
        }
    }

}