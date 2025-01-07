using UnityEngine;
[CreateAssetMenu(fileName = "BasicItem", menuName = "Custom/EquipmentData")]
public class EquipmentData: ItemData
{
    public ResistanceData resistanceData;
    public SpellDamageData damageData;
    public float armor;
    public float moveSpeed;
    public float healthPoints;
    
    public float manaAmp;
    public float passiveManaRegenAmp;
    
    public Spell sealedSpell;
}
