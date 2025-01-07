using System.Collections;
using UnityEngine;

public abstract class Spell : MonoBehaviour
{
    public SpellData spellData;
    public PlayerData playerData;
    public bool isSelected;
    public bool isMain = true;
    public float mana;
    protected Player Player; 
    protected Camera Camera;
    protected Transform SpellParent;

    public virtual float CastSpell()
    {
        mana -= spellData.manaCost;
        return spellData.spellCd;
    }

    protected bool EnoughMana()
    {
        if (mana < spellData.manaCost)
        {
            Debug.Log("NotEnoughMana");
            return false;
        }

        return true;
    }

    private void Awake()
    {
        SpellParent = GameObject.Find("Spell").transform;
        mana = spellData.manaPool * (1 + playerData.maxManaAmp);
        Player = FindObjectOfType<Player>();
        Camera = FindObjectOfType<Camera>();
    }

    private void Update()
    {
        if (!isMain)
        {
            return;
        }

        if (!isSelected && mana < spellData.manaPool)
        {
            PassiveManaRegen();
        }
    }

    private void PassiveManaRegen()
    {
        if (mana < spellData.manaPool)
        {
            mana += (1+playerData.passiveManaRegenAmp) * Time.deltaTime;
        }

        if (mana > spellData.manaPool)
        {
            mana = spellData.manaPool;
        }
    }

    protected Vector2 Dir;
    protected (Vector2, float) GetPositionAroundPlayerRelativeToMouse()
    {
        Dir = TransformUtil.GetDirFromPos(Player.transform.position, Camera.ScreenToWorldPoint(Input.mousePosition));
        Vector2 pos = Player.transform.position;
        return (Dir + pos, TransformUtil.GetRotFromDir(Dir));
    }
    
    protected IEnumerator DestroyAfter(float duration, GameObject objToDestroy)
    {
        yield return new WaitForSeconds(duration);
        Destroy(objToDestroy);
    }
    
    protected void SpawnParticle(float size, Vector3 pos, Quaternion rotation, bool flip)
    {
        GameObject pref = spellData.particlePref;
        GameObject particle = Instantiate(pref, pos, rotation).gameObject;
        particle.transform.localScale *= size;
        if (flip)
        {
            particle.GetComponent<SpriteRenderer>().flipY = true;
        }
    }
}
