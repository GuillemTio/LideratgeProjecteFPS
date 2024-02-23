using System.Collections.Generic;

public class WeaponPair
{
    public WeaponPair(List<Weapon> list) 
    {
        PrimaryWeapon = list[0];
        SecondaryWeapon = list[1];
    }

    public Weapon PrimaryWeapon;
    public Weapon SecondaryWeapon;

    public void SwapWeapons()
    {
        (PrimaryWeapon, SecondaryWeapon) = (SecondaryWeapon, PrimaryWeapon);
    }
}
