using UnityEngine;

class SniperDispersion : CommonWeaponDispersion
{
    public override float DispersionPerShot => m_Weapon.IsAiming ? 0f : base.DispersionPerShot;
    [SerializeField] private float m_WalkDispersion;
    [SerializeField] private float m_CameraDispersion;
    public override float GetCurrentDispersion(Weapon weapon)
    {
        var l_Disp = 0.0f;
        if (m_FPSController.m_RigidBody.velocity.magnitude > 0.1f)
        {
            l_Disp += m_WalkDispersion;
        }
        return l_Disp;
    }
}