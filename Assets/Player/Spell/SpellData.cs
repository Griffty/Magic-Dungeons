
using UnityEngine;

[CreateAssetMenu(fileName = "SpellData", menuName = "Custom/SpellData")]
public class SpellData : ScriptableObject
{
    public string spellName;
    public float manaCost;
    public float manaPool;
    public float spellCd;

    public float spellDamage;
    public float lifeTime;
    public GameObject particlePref;

    public float projectileSpeed;

    public Sprite spellIcon;
}
