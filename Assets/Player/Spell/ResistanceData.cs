
using System;
using UnityEngine;
[CreateAssetMenu(fileName = "ResistanceData", menuName = "Custom/ResistanceData")]
public class ResistanceData : ScriptableObject
{
    public float fireArmor;
    public float waterArmor;
    public float windArmor;
    public float earthArmor;
    public float lightningArmor;
    public float darkArmor;
    public float holyArmor;

    public void RecalculateAllResistance()
    {
        fireResistance = R(fireArmor);
        waterResistance = R(waterArmor);
        windResistance = R(windArmor);
        earthResistance = R(earthArmor);
        lightningResistance = R(lightningArmor);
        darkResistance = R(darkArmor);
        holyResistance = R(holyArmor);
    }

    private float R(float armor)
    {
        return armor/(armor + 40) * 0.9f;
    }
    
    public float fireResistance;
    public float waterResistance;
    public float windResistance;
    public float earthResistance;
    public float lightningResistance;
    public float darkResistance;
    public float holyResistance;
    
    public float GetResist(DamageType damageType)
    {
        return damageType switch
        {
            DamageType.Dark => darkResistance,
            DamageType.Earth => earthResistance,
            DamageType.Fire => fireResistance,
            DamageType.Holy => holyResistance,
            DamageType.Lightning => lightningResistance,
            DamageType.Water => waterResistance,
            DamageType.Wind => windResistance,
            _ => throw new ArgumentOutOfRangeException(nameof(damageType), damageType, null)
        };
    }
}
