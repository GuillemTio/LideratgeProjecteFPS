using System.Collections.Generic;

public class WeaponPair
{
    public WeaponPair(List<Weapon> list) 
    {
        PrimaryWeapon = list[0];
        SecondaryWeapon = list[1];

        SecondaryWeapon.SetEnabled(false);
        PrimaryWeapon.SetEnabled(true);
    }

    public Weapon PrimaryWeapon;
    public Weapon SecondaryWeapon;

    public void SwapWeapons()
    {
        var l_Temp = PrimaryWeapon;
        
        PrimaryWeapon = SecondaryWeapon;
        SecondaryWeapon = l_Temp;

        SecondaryWeapon.Draw();
        PrimaryWeapon.Undraw();
    }
}
