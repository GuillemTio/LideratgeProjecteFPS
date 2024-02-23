﻿using System;
using Unity.VisualScripting;
using UnityEngine;
public class SniperWeapon : Weapon
{
    protected override void Shoot()
    {
        base.Shoot();
        
        Vector3 l_CameraCenter = new(0.50f, 0.5f, 0.0f);
        Vector3 l_DispersionOffset = Holder.Crosshair.GetRandomPointInsideCrosshair();
        
        l_DispersionOffset.x /= Holder.RaycastCam.pixelWidth;
        l_DispersionOffset.y /= Holder.RaycastCam.pixelHeight;
        
        var l_Ray = Holder.RaycastCam.ViewportPointToRay(l_CameraCenter + l_DispersionOffset);
        
        if (Physics.Raycast(l_Ray, out RaycastHit l_RaycastHit, m_Range))
        {
            l_RaycastHit.transform.GetComponent<IShootable>()?.HandleShooted(m_Damage);
            Debug.DrawLine(l_RaycastHit.point, l_RaycastHit.point + l_RaycastHit.normal, Color.red, 5f);
        }
    }

}