using UnityEngine;

public class RocketLauncherAnimation : WeaponAnimation
{
     private new RocketLauncherWeapon m_Weapon;
     private static readonly int Reloading = Animator.StringToHash("Reloading");

     protected override void Awake()
     {
          base.Awake();
          m_Weapon = GetComponent<RocketLauncherWeapon>();
     }

     protected override void Update()
     {
          base.Update();
          m_Animator.SetBool(Reloading, m_Weapon.IsReloading);
     }
}