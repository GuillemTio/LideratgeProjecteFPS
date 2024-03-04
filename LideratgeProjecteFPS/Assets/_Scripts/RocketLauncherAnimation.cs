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

     protected override void OnEnable()
     {
          base.OnEnable();
          m_Weapon.OnReload += OnReload;
     }

     private void OnReload()
     {
          m_Animator.SetBool(Reloading, true);
     }

     protected override void OnSeath()
     {
          base.OnSeath();
          m_Animator.SetBool(Reloading, false);
     }
}