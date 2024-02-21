using System;
[Serializable]
public class CDispersionModifiers
{
    public enum Types {CamRotation, Movement, Sprinting, Jump }
    public Types m_Type;
    public float m_DispersionValue;
    public float GetDispersionAdded(Weapon weapon)
    {
        // if (m_FPSController.ModifierIsActive(m_Type))
        // {
        //     return m_DispersionValue;
        // }
        return 0.0f;
    }
}
