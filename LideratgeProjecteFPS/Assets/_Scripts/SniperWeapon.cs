using System;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
public class SniperWeapon : Weapon
{
    [SerializeField] private ParticleSystem m_MuzzleFlash;
    [SerializeField] private ParticleSystem m_Cartridge;
    
    protected override void Shoot()
    {
        base.Shoot();
        
        Vector3 l_CameraCenter = new(0.50f, 0.5f, 0.0f);
        Vector3 l_DispersionOffset = Holder.Crosshair.GetRandomPointInsideCrosshair();
        m_MuzzleFlash.Play();
        m_Cartridge.Play();
        l_DispersionOffset.x /= Holder.RaycastCam.pixelWidth;
        l_DispersionOffset.y /= Holder.RaycastCam.pixelHeight;
        
        var l_Ray = Holder.RaycastCam.ViewportPointToRay(l_CameraCenter + l_DispersionOffset);
        
        if (Physics.Raycast(l_Ray, out RaycastHit l_RaycastHit, m_Range, m_ShootableLayer))
        {
            l_RaycastHit.transform.GetComponent<IShootable>()?.HandleShooted(m_Damage);
            GameObject l_Decal;
            if (l_RaycastHit.transform.CompareTag("Metal"))
                l_Decal = Holder.MetaldDecalPool.GetNextElement();
            else
                l_Decal = Holder.StoneDecalPool.GetNextElement();
            l_Decal.transform.position = l_RaycastHit.point;
            l_Decal.transform.rotation = Quaternion.LookRotation(l_RaycastHit.normal);
            l_Decal.SetActive(false);                
            l_Decal.SetActive(true);                
            l_Decal.transform.parent = l_RaycastHit.transform;
        }
    }

}