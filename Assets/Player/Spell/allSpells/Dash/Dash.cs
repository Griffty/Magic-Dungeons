using System.Collections;
using UnityEngine;

public class Dash : Spell
{
    public bool isDashing;

    public override float CastSpell()
    {
        if (!EnoughMana())
        {
            return -1;
        }
        StartCoroutine(MakeDash());
        return base.CastSpell();
    }
    
    IEnumerator MakeDash()
    {
        float startTime = Time.time;
        isDashing = true;
        while (Time.time < startTime + spellData.lifeTime)
        {
            yield return null;
        }
        isDashing = false;
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            Player.rb.velocity *= spellData.spellDamage;
        }
    }
}