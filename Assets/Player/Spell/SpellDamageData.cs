using System;
using UnityEngine;

[CreateAssetMenu(fileName = "SpellDamageData", menuName = "Custom/SpellDamageData")]
public class SpellDamageData : ScriptableObject
{
    public float fireDamage = 1;
    public float waterDamage = 1;
    public float windDamage = 1;
    public float earthDamage = 1;
    public float lightningDamage = 1;
    public float darkDamage = 1;
    public float holyDamage = 1;

    public float GetAmp(DamageType damageType)
    {
        return damageType switch
        {
            DamageType.Dark => darkDamage,
            DamageType.Earth => earthDamage,
            DamageType.Fire => fireDamage,
            DamageType.Holy => holyDamage,
            DamageType.Lightning => lightningDamage,
            DamageType.Water => waterDamage,
            DamageType.Wind => windDamage,
            _ => throw new ArgumentOutOfRangeException(nameof(damageType), damageType, null)
        };
    }
}