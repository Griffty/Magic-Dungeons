using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindBurst : Spell
{
    private Collider2D _collider2D;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    private void Start()
    {
        GetComponent<Rigidbody2D>();
        _collider2D = GetComponent<Collider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }
    public override float CastSpell()
    {
        if (!EnoughMana())
        {
            return -1;
        }
        ShootWindBurst();
        return base.CastSpell();
    }

    private void ShootWindBurst()
    {
        (Vector3 bPos, float bRot) = GetPositionAroundPlayerRelativeToMouse(); 
        GameObject windBurst = Instantiate(gameObject, bPos, Player.transform.rotation);
        
        windBurst.transform.RotateAround(bPos, Vector3.forward, bRot);
        windBurst.transform.SetParent(Player.transform);

        Animator animator = windBurst.GetComponent<Animator>();
        animator.enabled = true;

        windBurst.GetComponent<Collider2D>().enabled = true;
        windBurst.GetComponent<SpriteRenderer>().enabled = true;
        windBurst.GetComponent<WindBurst>().isMain = false;
        
        StartCoroutine(CastTime(0.6f, animator, windBurst.GetComponent<Collider2D>()));
    }

    private IEnumerator CastTime(float time, Animator animator, Collider2D collider2D)
    {
        yield return new WaitForSeconds(time);
        collider2D.enabled = true;
        animator.SetBool("Fly", true);
        GameObject o;
        (o = animator.gameObject).transform.SetParent(SpellParent);
        int hitsCount = 0;
        StartCoroutine(MakeHit(hitsCount, 0.6f, o));
    }

    private IEnumerator MakeHit(int hitsCount, float time, GameObject spell)
    {
        DealDamage(spell);
        hitsCount++;
        yield return new WaitForSeconds(time);
        if (hitsCount > spellData.lifeTime)
        {
            Destroy(spell);
            yield break;
        }

        StartCoroutine(MakeHit(hitsCount, time, spell));
    }

    private void DealDamage(GameObject spell)
    {
        foreach (var col in spell.GetComponent<WindBurst>()._colliders)
        {
            if (col == null)
            {
                continue;
            }
            if (col.CompareTag("Player") || col.isTrigger)
            {
                continue;
            }
            if (col.TryGetComponent(out HealthHandler healthHandler))
            {
                healthHandler.TakeDamage(spellData.spellDamage, DamageType.Wind, true);
            }
            
            var position = col.transform.position;
            bool flip = position.x - transform.position.x < 0;

            float Rot = Mathf.Atan2(Dir.y, Dir.x) * Mathf.Rad2Deg;
            
            Quaternion rot = new Quaternion
            {
                eulerAngles = new Vector3(0, 0, Rot)
            };
            SpawnParticle(1, position, rot, flip);
        }
    }

    private void FixedUpdate()
    {
        if (isMain)
        {
            return;
        }
        
        if (!_animator.GetBool("Fly"))
        {
            (Vector3 bPos, float bRot) = GetPositionAroundPlayerRelativeToMouse(); 
    
            _spriteRenderer.flipY = bRot is < -90 or > 90 ;

            var trb = transform;
            trb.position = bPos;
            trb.eulerAngles = new Vector3(0, 0, bRot);
        }
    }
    
    private readonly HashSet<Collider2D> _colliders = new ();

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Player"))
        {
            _colliders.Add(col); 
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
        {
            _colliders.Remove(other); 
        }
    }
}
